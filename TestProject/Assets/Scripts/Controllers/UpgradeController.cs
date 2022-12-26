using System;
using Context;
using Data;
using HoldersAndSettings;
using Random = UnityEngine.Random;

namespace Controllers
{
    public interface IProcessUpgrade
    {
        void ProcessUpgrade();
    }

    public class UpgradeController : ControllersBase, IProcessUpgrade
    {
        private IDataRepository<PlayerData> _playerDataRepository;
        private ILoadSettings<UpgradeSettings> _loadSettings;
        private UpgradeData[] _upgradeData;

        public void ProcessUpgrade()
        {
            var random = Random.Range(0f, 100f);

            UpgradeData currentData = null;

            for (int i = 0; i < _upgradeData.Length; i++)
            {
                var currData = _upgradeData[i];

                if (random >= currData.Chance)
                    continue;

                currentData = currData;
                Upgrade(currentData);
                break;
            }

            if (currentData is null)
                Upgrade(_upgradeData[^1]);
        }

        public override void Init()
        {
            _playerDataRepository = SceneContext.Instance.Get<IDataRepository<PlayerData>>();
            _loadSettings = ProjectContext.Instance.Get<ILoadSettings<UpgradeSettings>>();
            _upgradeData =
                _loadSettings.LoadSettings().Load(runAsync: false).Result.UpgradeData.Clone() as UpgradeData[];
            Array.Sort(_upgradeData, (current, next) => current.Chance.CompareTo(next.Chance));

            for (int i = 1; i < _upgradeData.Length; i++)
            {
                _upgradeData[i].Chance += _upgradeData[i - 1].Chance;
            }
        }

        private void Upgrade(UpgradeData data)
        {
            switch (data.UpgradeType.Value)
            {
                case UpgradeType.Damage:
                    _playerDataRepository.Data.DamageAmount += data.Step;
                    break;

                case UpgradeType.Speed:
                    _playerDataRepository.Data.Speed += data.Step;
                    break;

                case UpgradeType.Radius:
                    _playerDataRepository.Data.Radius += data.Step;
                    break;
            }
        }
    }
}