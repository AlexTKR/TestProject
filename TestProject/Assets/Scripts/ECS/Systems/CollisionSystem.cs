using System;
using Context;
using Data;
using ECS.Components;
using ECS.Providers;
using Extensions;
using Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(CollisionSystem))]
    public sealed class CollisionSystem : FixedUpdateSystem
    {
        private Filter _filter;
        private IDataRepository<PlayerData> _dataRepository;
        private LayerMask _layerMask;
        private static Comparison<(int index, float distance)> _comparison;

        private Collider[] _hits
            = new Collider[10];

        public override void OnAwake()
        {
            _comparison = (curr, next) => curr.distance.CompareTo(next.distance);
            _filter = World.Filter.With<PlayerComponent>().With<TransformComponent>();
            _dataRepository = SceneContext.Instance.GetSingle<IDataRepository<PlayerData>>();
            _layerMask = LayerMask.GetMask("Enemy");
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_filter.IsEmpty())
                return;

            _hits.ClearOptimized();

            var playerEntity = _filter.First();
            ref var playerTransformComponent = ref playerEntity.GetComponent<TransformComponent>();

            var size = Physics.OverlapSphereNonAlloc(playerTransformComponent.Transform.position,
                _dataRepository.Data.Radius, _hits, _layerMask);

            if (size == 0)
                return;

            if (size <= RuntimeData.MaxDamageCount)
            {
                for (int i = 0; i < size; i++)
                {
                    ApplyDamage(_hits, i);
                }

                return;
            }

            Span<(int index, float distance)> indexToDistance = stackalloc (int Index, float distande)[size];
            GetClosestTargets(_hits, ref size, ref playerTransformComponent, ref indexToDistance);

            for (int i = 0; i < RuntimeData.MaxDamageCount; i++)
            {
                var index = indexToDistance[i].index;
                ApplyDamage(_hits, index);
            }
        }

        private static void GetClosestTargets(Collider[] hits, ref int size,
            ref TransformComponent playerTransformComponent,
            ref Span<(int, float)> indexToDistance)
        {
            for (int i = 0; i < size; i++)
            {
                if (!hits[i].TryGetComponent<CollisionProvider>(out var provider))
                    continue;

                ref var transformComponent = ref provider.EntityInstance.GetComponent<TransformComponent>();
                var offset = playerTransformComponent.Transform.position - transformComponent.Transform.position;
                var distance = Vector3.SqrMagnitude(offset);
                indexToDistance[i] = (i, distance);
            }

            indexToDistance.SortSpan(_comparison);
        }

        private static void ApplyDamage(Collider[] hits, int index)
        {
            if (!hits[index].TryGetComponent<CollisionProvider>(out var provider))
                return;

            var entity = provider.EntityInstance;

            if (entity.Has<DamageDelayComponent>())
            {
                ref var damageDelayComponent = ref entity.GetComponent<DamageDelayComponent>();
                if (DateTime.Now.TimeOfDay < damageDelayComponent.DelayTime)
                    return;

                entity.RemoveComponent<DamageDelayComponent>();
            }

            entity.SetComponent(new DamageDelayComponent()
                { DelayTime = DateTime.Now.TimeOfDay + TimeSpan.FromSeconds(1d) });
            entity.SetComponent(new DamageComponent());
        }
    }
}