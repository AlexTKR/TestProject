using System;
using System.Collections.Generic;
using ECS.Components;
using Morpeh;
using UnityEngine;

namespace ECS.Pools
{
    public class SimpleEnemyPool<T> : IEnemyPool<T> where T : class
    {
        private Dictionary<EnemyType, List<T>> _container =
            new Dictionary<EnemyType, List<T>>();

        public SimpleEnemyPool()
        {
            foreach (var enemyType in Enum.GetValues(typeof(EnemyType)))
            {
                _container[(EnemyType)enemyType] = new List<T>();
            }
        }

        public T Get(EnemyType enemyType)
        {
            if (_container[enemyType].Count == 0)
                return null;

            var entity = _container[enemyType][0];
            _container[enemyType].RemoveAt(0);
            return entity;
        }

        public void Return(T entity, EnemyType type)
        {
            _container[type].Add(entity);
        }
    }
}