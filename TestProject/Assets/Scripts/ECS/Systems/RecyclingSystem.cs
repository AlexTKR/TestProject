using System.Linq;
using Context;
using ECS.Components;
using ECS.Pools;
using Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(RecyclingSystem))]
    public sealed class RecyclingSystem : UpdateSystem
    {
        private Filter _filter;
        private IEnemyPool<GameObject> _enemyPool;

        public override void OnAwake()
        {
            _filter = World.Filter.With<RecyclingComponent>().With<ActiveComponent>()
                .With<GameobjectComponent>();
            _enemyPool = SceneContext.Instance.GetSingle<IEnemyPool<GameObject>>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_filter.IsEmpty())
                return;

            foreach (var entity in _filter)
            {
                ref var transformComponent = ref entity.GetComponent<TransformComponent>();
                if (entity.Has<EnemyComponent>())
                {
                    ref var enemyComponent = ref entity.GetComponent<EnemyComponent>();
                    ref var gameobjectComponent = ref entity.GetComponent<GameobjectComponent>();
                    _enemyPool.Return(gameobjectComponent.GameObject, enemyComponent.EnemyType);
                }

                entity.RemoveComponent<RecyclingComponent>();
                entity.RemoveComponent<ActiveComponent>();

                transformComponent.Transform.gameObject.SetActive(false);
            }
        }
    }
}