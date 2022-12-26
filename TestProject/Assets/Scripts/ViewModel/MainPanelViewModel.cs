using System;
using Context;
using Controllers;
using Data;
using TMPro;
using UniMob;

namespace ViewModel
{
    public interface IMainPanel
    {
        TextMeshProUGUI SpeedText { get; }
        TextMeshProUGUI RadiusText { get; }
        TextMeshProUGUI DamageText { get; }
        TextMeshProUGUI KillCountText { get; }

        event Action OnUpgrade;
    }

    public class MainPanelViewModel : ViewModelBase<IMainPanel>
    {
        private IDataRepository<PlayerData> _playerDataRepository;
        private IDataRepository<PlayerKillData> _playerKillDataRepository;
        private IProcessUpgrade _processUpgrade;

        public override void Compose(Lifetime lifetime, IMainPanel panel)
        {
            _playerDataRepository = SceneContext.Instance.Get<IDataRepository<PlayerData>>();
            _playerKillDataRepository = SceneContext.Instance.Get<IDataRepository<PlayerKillData>>();
            _processUpgrade = SceneContext.Instance.Get<IProcessUpgrade>();

            Atom.Reaction(lifetime,
                () => { panel.DamageText.text = _playerDataRepository.Data.DamageAmount.ToString("F1"); });
            Atom.Reaction(lifetime, () => { panel.SpeedText.text = _playerDataRepository.Data.Speed.ToString("F1"); });
            Atom.Reaction(lifetime,
                () => { panel.RadiusText.text = _playerDataRepository.Data.Radius.ToString("F1"); });
            Atom.Reaction(lifetime,
                () => { panel.KillCountText.text = _playerKillDataRepository.Data.KillCount.ToString(); });
            panel.OnUpgrade += () => { _processUpgrade.ProcessUpgrade(); };
        }
    }
}