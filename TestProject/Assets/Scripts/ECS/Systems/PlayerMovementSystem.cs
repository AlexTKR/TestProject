using Context;
using Data;
using ECS.Components;
using Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(PlayerMovementSystem))]
    public sealed class PlayerMovementSystem : UpdateSystem
    {
        private Filter _filter;
        private IDataRepository<PlayerData> _dataRepository;

        public override void OnAwake()
        {
            _filter = World.Filter.With<PlayerComponent>().With<TransformComponent>().With<MoveByInputComponent>();
            _dataRepository = SceneContext.Instance.Get<IDataRepository<PlayerData>>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_filter.IsEmpty() || _dataRepository is null)
                return;

            var entity = _filter.First();
            ref var transformComponent = ref entity.GetComponent<TransformComponent>();
            ref var moveByInputComponent = ref entity.GetComponent<MoveByInputComponent>();
            transformComponent.Transform.position +=
                moveByInputComponent.Direction * _dataRepository.Data.Speed * deltaTime;

            entity.RemoveComponent<MoveByInputComponent>();
        }
    }
}