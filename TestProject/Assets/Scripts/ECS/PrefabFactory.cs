using UnityEngine;
using ECS.Components;
using Morpeh;

namespace ECS
{
    public interface IEntityFactory<T, T1>
        where T1 : struct
    {
        void Get(T entity, ref T1 spawnComponent);
    }

    public class PrefabFactory : IEntityFactory<Entity, SpawnComponent>
    {
        public void Get(Entity entity, ref SpawnComponent spawnComponent)
        {
            var spawnObject = Object.Instantiate(spawnComponent.Prefab, spawnComponent.Position,
                spawnComponent.Rotation, spawnComponent.Parent);
            spawnObject.SetActive(spawnComponent.IsActive);

            entity.RemoveComponent<SpawnComponent>();
        }
    }
}