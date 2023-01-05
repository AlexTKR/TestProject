using CommonBehaviours;
using Context;
using Controllers;
using Data;
using ECS;
using ECS.Pools;
using HoldersAndSettings;
using UnityEngine;
using View;
using ViewModel;

namespace Composition
{
    public class CompositionRoot : MonoBehaviour
    {
        [SerializeField] private PanelBase[] _panels;

        private void Start()
        {
            RegisterLoadables();
            RegisterControllers();
            RegisterData();
            RegisterViewModels();
            RegisterSettings();
            RegisterPools();

            var initer = new Initer();
            initer.Init();
        }

        private void RegisterPools()
        {
            SceneContext.Instance.RegisterAsSingle<IEnemyPool<GameObject>>(
                new SimpleEnemyPool());
        }

        private void RegisterControllers()
        {
            var processUpgrade = new UpgradeController();
            var bundleController = new BundleController();

            ProjectContext.Instance.RegisterAsSingle(bundleController.GetType().GetInterfaces(), bundleController);
            SceneContext.Instance.RegisterAsSingle<IGetScreenBounds>(new CameraController());
            SceneContext.Instance.RegisterAsSingle<IProcessUpgrade>(processUpgrade);

            SceneContext.Instance.RegisterAsMultiple<IInit>(new object[] { processUpgrade });
        }

        private void RegisterSettings()
        {
            var gameSettings = ProjectContext.Instance.Get<ILoadSettings<GameSettings>>().LoadSettings()
                .Load(runAsync: false)
                .Result;
            RuntimeData.MaxEnemyCount = gameSettings.EnemyCount;
            RuntimeData.MaxDamageCount = gameSettings.MaxDamageCount;
        }

        private void RegisterData()
        {
            var playerDataSource = ProjectContext.Instance.Get<ILoadSource<PlayerDataSource>>().LoadSource()
                .Load(runAsync: false)
                .Result;

            SceneContext.Instance.RegisterAsSingle<IDataRepository<PlayerData>>(
                new PlayerDataRepository(new SoDataSourceProvider<PlayerData>(playerDataSource)));
            SceneContext.Instance.RegisterAsSingle<IDataRepository<PlayerKillData>>(new PlayerKillRepository());
        }

        private void RegisterViewModels()
        {
            var viewModelProvider = new ViewModelProvider();
            SceneContext.Instance.RegisterAsSingle<IGetViewModel>(viewModelProvider);

            for (int i = 0; i < _panels.Length; i++)
            {
                _panels[i].Init(viewModelProvider);
            }
        }

        private void RegisterLoadables()
        {
            var bundleController = new BundleController();

            foreach (var type in bundleController.GetType().GetInterfaces())
            {
                ProjectContext.Instance.RegisterAsSingle(type, bundleController);
            }
        }

        private void OnDestroy()
        {
            SceneContext.Instance.ClearSceneContext();
        }
    }
}