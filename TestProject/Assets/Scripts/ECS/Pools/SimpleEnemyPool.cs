using System;
using System.Collections.Generic;
using ECS.Components;
using UnityEngine;

namespace ECS.Pools
{
    public class SimpleEnemyPool : IEnemyPool<GameObject>
    {
        private IEntityPool<GameObject> _redEnemyPool = new SimpleEntityPool<GameObject>();
        private IEntityPool<GameObject> _yellowEnemyPool = new SimpleEntityPool<GameObject>();
        private IEntityPool<GameObject> _greenEnemyPool = new SimpleEntityPool<GameObject>();


        public GameObject Get(EnemyType enemyType)
        {
            var entity = enemyType switch
            {
                EnemyType.Red => _redEnemyPool.Get(),
                EnemyType.Yellow => _yellowEnemyPool.Get(),
                EnemyType.Green => _greenEnemyPool.Get(),
            };

            return entity;
        }

        public void Return(GameObject entity, EnemyType enemyType)
        {
            var pool = enemyType switch
            {
                EnemyType.Red => _redEnemyPool,
                EnemyType.Yellow => _yellowEnemyPool,
                EnemyType.Green => _greenEnemyPool,
            };

            pool.Return(entity);
        }
    }
}