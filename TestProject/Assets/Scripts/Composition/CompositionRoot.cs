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

        private IProcessUpgrade _processUpgrade;
        private IInit _initer;

        private void Start()
        {
            RegisterControllers();
            RegisterLoadables();
            RegisterData();
            RegisterViewModels();
            RegisterSettings();
            RegisterPools();

            _initer = new Initer((IInit)_processUpgrade);
            _initer.Init();
        }

        private void RegisterPools()
        {
            SceneContext.Instance.Register<IEnemyPool<GameObject>>(
                new SimpleEnemyPool<GameObject>());
        }

        private void RegisterControllers()
        {
            _processUpgrade = new UpgradeController();
            var bundleController = new BundleController();

            ProjectContext.Instance.Register(bundleController.GetType().GetInterfaces(), bundleController);
            SceneContext.Instance.Register<IGetScreenBounds>(new CameraController());
            SceneContext.Instance.Register<IProcessUpgrade>(_processUpgrade);
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

            SceneContext.Instance.Register<IDataRepository<PlayerData>>(
                new PlayerDataRepository(new SoDataSourceProvider<PlayerData>(playerDataSource)));
            SceneContext.Instance.Register<IDataRepository<PlayerKillData>>(new PlayerKillRepository());
        }

        private void RegisterViewModels()
        {
            var viewModelProvider = new ViewModelProvider();
            SceneContext.Instance.Register<IGetViewModel>(viewModelProvider);

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
                ProjectContext.Instance.Register(type, bundleController);
            }
        }

        private void OnDestroy()
        {
            SceneContext.Instance.ClearSceneContext();
        }
    }
}