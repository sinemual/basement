using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;

namespace Client
{
    public class TntPickaxeScreenInputSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private UserInterfaceEventBus _userInterfaceEventBus;
        private AnalyticService _analyticService;
        private VibrationService _vibrationService;
        private AudioService _audioService;
        private AdsService _adsService;
        
        public void Init()
        {
            _userInterfaceEventBus.TntPickaxeBoosterScreen.ActivateTntPickaxeBoosterButtonTap += () =>
            {
#if UNITY_EDITOR
                GetReward();
#endif
                _adsService.ShowRewardVideo("get_tnt_pickaxe", GetReward);
            };
        }

        private void GetReward()
        {
            _data.RuntimeData.IsTntPickaxeBoosterWork = true;
            _ui.BoosterFeelingScreen.SetShowState(true);
        }
    }
}