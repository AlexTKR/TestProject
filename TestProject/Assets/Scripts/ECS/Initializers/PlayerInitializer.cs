using Context;
using Controllers;
using Data;
using ECS.Components;
using Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Initializers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(PlayerInitializer))]
    public sealed class PlayerInitializer : Initializer
    {
        private IDataRepository<PlayerData> _dataRepository;

        public override void OnAwake()
        {
            _dataRepository = SceneContext.Instance.Get<IDataRepository<PlayerData>>();
            var playerGameObject = ProjectContext.Instance.Get<ILoadPlayer>().LoadPLayer().Load(runAsync: false).Result;
            World.CreateEntity().SetComponent(new SpawnComponent() { Prefab = playerGameObject , IsActive = true});
        }

        public override void Dispose()
        {
        }
    }
}