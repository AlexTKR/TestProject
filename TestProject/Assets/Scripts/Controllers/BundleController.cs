using System.Threading.Tasks;
using DataSource;
using Holders;
using HoldersAndSettings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public interface ILoadable<T>
{
    Task<T> Load(bool autoRelease = true, bool runAsync = true);
    void Release();
}

public class LoadReference<T, TIn> : ILoadable<T> where T : class
{
    private string _id;
    private AsyncOperationHandle _handle;

    public LoadReference(string id)
    {
        _id = id;
    }

    public async Task<T> Load(bool autoRelease = true, bool runAsync = true)
    {
        _handle = Addressables.LoadAssetAsync<TIn>(_id);

        if (runAsync)
            await _handle.Task;
        else
            _handle.WaitForCompletion();

        var result = CastResult();

        if (autoRelease)
        {
            Release();
        }

        return result;
    }

    private T CastResult()
    {
        return typeof(TIn) == typeof(GameObject) && typeof(T) != typeof(TIn)
            ? ((GameObject)_handle.Result).GetComponent<T>()
            : (T)_handle.Result;
    }

    public void Release()
    {
        Addressables.Release(_handle);
    }
}

namespace Controllers
{
    public interface ILoadSource<T>
    {
        ILoadable<T> LoadSource();
    }

    public interface ILoadPlayer
    {
        ILoadable<GameObject> LoadPLayer();
    }

    public interface ILoadEnemies
    {
        ILoadable<PrefabHolder> LoadEnemies();
    }

    public interface ILoadSettings<T>
    {
        ILoadable<T> LoadSettings();
    }

    public class BundleController : ILoadPlayer, ILoadSource<PlayerDataSource>, ILoadEnemies,
        ILoadSettings<GameSettings>, ILoadSettings<UpgradeSettings>
    {
        private ILoadable<GameObject> _playerPrefab;
        private ILoadable<PlayerDataSource> _playerSettings;
        private ILoadable<PrefabHolder> _enemyHolder;
        private ILoadable<GameSettings> _gameSettings;
        private ILoadable<UpgradeSettings> _upgradeSettings;

        public ILoadable<GameObject> LoadPLayer()
        {
            return _playerPrefab ??= new LoadReference<GameObject, GameObject>("Player");
        }

        public ILoadable<PlayerDataSource> LoadSource()
        {
            return _playerSettings ??= new LoadReference<PlayerDataSource, PlayerDataSource>("PlayerDataSource");
        }


        public ILoadable<PrefabHolder> LoadEnemies()
        {
            return _enemyHolder ??= new LoadReference<PrefabHolder, PrefabHolder>("EnemyHolder");
        }


        public ILoadable<GameSettings> LoadSettings()
        {
            return _gameSettings ??= new LoadReference<GameSettings, GameSettings>("GameSettings");
        }

        ILoadable<UpgradeSettings> ILoadSettings<UpgradeSettings>.LoadSettings()
        {
            return _upgradeSettings ??= new LoadReference<UpgradeSettings, UpgradeSettings>("UpgradeSettings");
        }
    }
}