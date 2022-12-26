using System;
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
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EnemyDamageSystem))]
    public sealed class EnemyDamageSystem : UpdateSystem
    {
        private Filter _filter;
        private IDataRepository<PlayerData> _dataRepository;
        private IDataRepository<PlayerKillData> _playerKillDataRepository;

        public override void OnAwake()
        {
            _filter = World.Filter.With<EnemyComponent>().With<DamageComponent>()
                .With<HealthComponent>();
            _dataRepository = SceneContext.Instance.Get<IDataRepository<PlayerData>>();
            _playerKillDataRepository = SceneContext.Instance.Get<IDataRepository<PlayerKillData>>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var healthComponent = ref entity.GetComponent<HealthComponent>();
                healthComponent.CurrentHealth = Math.Clamp(
                    healthComponent.CurrentHealth - _dataRepository.Data.DamageAmount, 0,
                    healthComponent.InitialHealth);
                if (healthComponent.CurrentHealth == 0)
                {
                    entity.SetComponent(new RecyclingComponent());
                    _playerKillDataRepository.Data.KillCount++;
                }

                entity.RemoveComponent<DamageComponent>();
            }
        }
    }
}