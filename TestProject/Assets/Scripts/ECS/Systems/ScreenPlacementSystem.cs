using Context;
using Controllers;
using ECS.Components;
using Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ScreenPlacementSystem))]
    public sealed class ScreenPlacementSystem : UpdateSystem
    {
        private Vector2 _screenBounds;
        private Filter _filter;

        public override void OnAwake()
        {
            _filter = World.Filter.With<ScreenPlacementComponent>().With<TransformComponent>()
                .With<ColliderComponent>();
            _screenBounds = SceneContext.Instance.Get<IGetScreenBounds>().ScreenBounds;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_filter.IsEmpty())
                return;

            foreach (var entity in _filter)
            {
                ref var transformComponent = ref entity.GetComponent<TransformComponent>();
                ref var colliderComponent = ref entity.GetComponent<ColliderComponent>();
                var collider = colliderComponent.Collider.bounds.size / 2f;
                var x = Random.Range(-_screenBounds.x + collider.x, _screenBounds.x - collider.x);
                var z = Random.Range(-_screenBounds.y + collider.x, _screenBounds.y - collider.x);
                transformComponent.Transform.position = new Vector3(x, 0f, z);

                entity.SetComponent(new ActiveComponent());
                entity.RemoveComponent<ScreenPlacementComponent>();
            }
        }
    }
}