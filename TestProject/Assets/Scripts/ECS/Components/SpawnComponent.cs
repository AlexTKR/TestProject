using Morpeh;
using UnityEngine;

namespace ECS.Components
{
    public struct SpawnComponent : IComponent
    {
        public GameObject Prefab;
        public Vector3 Position;
        public Quaternion Rotation;
        public Transform Parent;
        public bool IsActive;
    }
}
