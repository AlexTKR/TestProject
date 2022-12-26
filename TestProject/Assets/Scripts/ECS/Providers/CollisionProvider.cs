using ECS.Components;
using Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CollisionProvider : MonoProvider<ColliderComponent>
    {
        [SerializeField] private Collider _collider;

        public Entity EntityInstance;

        protected override void Initialize()
        {
            base.Initialize();
            EntityInstance = Entity;
            ref var colliderComponent = ref Entity.GetComponent<ColliderComponent>();
            colliderComponent.Collider = _collider;
        }
    }
}