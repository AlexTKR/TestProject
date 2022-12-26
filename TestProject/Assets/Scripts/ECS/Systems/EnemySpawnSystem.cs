using System;
using System.Collections.Generic;
using System.Linq;
using Context;
using Controllers;
using ECS.Components;
using ECS.Pools;
using ECS.Providers;
using Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EnemySpawnSystem))]
    public sealed class EnemySpawnSystem : UpdateSystem
    {
        private Filter _filter;
        private IEnemyPool<GameObject> _enemyPool;

        private Dictionary<EnemyType, GameObject> _typeToPrefab =
            new Dictionary<EnemyType, GameObject>();

        private EnemyType[] _enemyTypes;

        public override void OnAwake()
        {
            _filter = World.Filter.With<EnemyComponent>().With<ActiveComponent>();
            _enemyPool = SceneContext.Instance.Get<IEnemyPool<GameObject>>();
            var enemyPrefabs = ProjectContext.Instance.Get<ILoadEnemies>().LoadEnemies().Load(runAsync: false).Result
                .Prefabs;

            _enemyTypes = Enum.GetValues(typeof(EnemyType)) as EnemyType[];

            for (int i = 0; i < enemyPrefabs.Length; i++)
            {
                var current = enemyPrefabs[i];
                if (!current.TryGetComponent<EnemyProvider>(out var provider))
                    continue;

                _typeToPrefab[provider.EnemyType] = current;
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_filter.Count() >= RuntimeData.MaxEnemyCount)
                return;

            var randomType = _enemyTypes[Random.Range(0, _enemyTypes.Length)];

            var go = _enemyPool.Get(randomType);
            if (go != null)
            {
                go.SetActive(true);
                return;
            }

            World.CreateEntity().SetComponent(new SpawnComponent()
            {
                Prefab = _typeToPrefab[randomType],
                Position = new Vector3(0f, 10f, 0f),
                IsActive = true
            });
        }
    }
}