using System;
using System.Linq;
using Context;
using Data;
using ECS.Components;
using ECS.Providers;
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

        public override void OnAwake()
        {
            _filter = World.Filter.With<PlayerComponent>().With<TransformComponent>();
            _dataRepository = SceneContext.Instance.GetSingle<IDataRepository<PlayerData>>();
            _layerMask = LayerMask.GetMask("Enemy");
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_filter.IsEmpty())
                return;

            var playerEntity = _filter.First();
            ref var playerTransformComponent = ref playerEntity.GetComponent<TransformComponent>();

            var hits = Physics.OverlapSphere(playerTransformComponent.Transform.position, _dataRepository.Data.Radius,
                _layerMask);

            if (hits.Length <= RuntimeData.MaxDamageCount)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    ApplyDamage(hits, i);
                }

                return;
            }

            var indexToDistance = new (int Index, float distande)[hits.Length];

            GetClosestTargets(hits, ref playerTransformComponent, indexToDistance);

            for (int i = 0; i < RuntimeData.MaxDamageCount; i++)
            {
                var index = indexToDistance[i].Index;
                ApplyDamage(hits, index);
            }
        }

        private static void GetClosestTargets(Collider[] hits, ref TransformComponent playerTransformComponent,
            (int Index, float distande)[] indexToDistance)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (!hits[i].TryGetComponent<CollisionProvider>(out var provider))
                    continue;

                ref var transformComponent = ref provider.EntityInstance.GetComponent<TransformComponent>();
                var offset = playerTransformComponent.Transform.position - transformComponent.Transform.position;
                var distance = Vector3.SqrMagnitude(offset);
                indexToDistance[i] = (i, distance);
            }

            Array.Sort(indexToDistance, (curr, next) => curr.distande.CompareTo(next.distande));
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