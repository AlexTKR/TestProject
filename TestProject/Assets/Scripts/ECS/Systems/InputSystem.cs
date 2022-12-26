using ECS.Components;
using Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InputSystem))]
    public sealed class InputSystem : UpdateSystem
    {
        private Filter _filter;

        public override void OnAwake()
        {
            _filter = World.Filter.With<PlayerComponent>().With<TransformComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_filter.IsEmpty())
                return;

            var entity = _filter.First();
            var direction = Vector3.zero;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                direction = Vector3.forward;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                direction = Vector3.right;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                direction = Vector3.left;
            }
            
            if (Input.GetKey(KeyCode.DownArrow))
            {
                direction = Vector3.back;
            }

            if (direction == Vector3.zero)
                return;

            var moveByInputComponent = new MoveByInputComponent() { Direction = direction };
            entity.SetComponent(moveByInputComponent);
        }
    }
}