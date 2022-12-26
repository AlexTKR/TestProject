using Morpeh;
using UnityEngine;

namespace ECS.Components
{
    public struct ColliderComponent : IComponent
    {
        public Collider Collider;
    }
}
