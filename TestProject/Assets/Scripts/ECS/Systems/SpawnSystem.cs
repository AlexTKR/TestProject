using ECS.Components;
using Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(SpawnSystem))]
    public sealed class SpawnSystem : UpdateSystem
    {
        private Filter _filter;
        private IEntityFactory<Entity, SpawnComponent> _entityFactory;

        public override void OnAwake()
        {
            _filter = World.Filter.With<SpawnComponent>();
            _entityFactory = new PrefabFactory();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_filter.IsEmpty())
                return;

            foreach (var entity in _filter)
            {
                ref var spawnComponent = ref entity.GetComponent<SpawnComponent>();
                _entityFactory.Get(entity, ref spawnComponent);
            }
        }
    }
}