using ECS.Components;
using Morpeh;
using UnityEngine;

namespace ECS.Pools
{
    public interface IEnemyPool<T>
    {
        T Get(EnemyType enemyType);
        void Return(T entity , EnemyType enemyType);
    }
}