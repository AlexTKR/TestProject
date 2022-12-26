using System;
using TMPro;
using UniMob;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;

namespace View
{
    public class MainPanel : PanelBase, IMainPanel
    {
        [SerializeField] private TextMeshProUGUI _speedText;
        [SerializeField] private TextMeshProUGUI _radiusText;
        [SerializeField] private TextMeshProUGUI _damageText;
        [SerializeField] private TextMeshProUGUI _killCountText;
        [SerializeField] private Button _upgradeButton;

        public TextMeshProUGUI SpeedText => _speedText;
        public TextMeshProUGUI RadiusText => _radiusText;
        public TextMeshProUGUI DamageText => _damageText;
        public TextMeshProUGUI KillCountText => _killCountText;

        public event Action OnUpgrade;

        private ViewModelBase<IMainPanel> _viewModel;

        public override void Init(IGetViewModel getViewModel)
        {
            _upgradeButton.onClick.AddListener(() => { OnUpgrade?.Invoke(); });
            _viewModel = getViewModel.Get<ViewModelBase<IMainPanel>>();
            _viewModel.Compose(Lifetime, this);
        }
    }
}