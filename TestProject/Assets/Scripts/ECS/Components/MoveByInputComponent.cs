using Morpeh;
using UnityEngine;

namespace ECS.Components
{
    public struct MoveByInputComponent : IComponent
    {
        public Vector3 Direction;
    }
}