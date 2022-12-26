using ECS.Components;
using Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HealthProvider : MonoProvider<HealthComponent>
    {
        [SerializeField] private float _healthAmount;

        protected override void Initialize()
        {
            base.Initialize();
            ref var healthComponent = ref Entity.GetComponent<HealthComponent>();
            healthComponent.InitialHealth = _healthAmount;
            healthComponent.CurrentHealth = healthComponent.InitialHealth;
        }
    }
}