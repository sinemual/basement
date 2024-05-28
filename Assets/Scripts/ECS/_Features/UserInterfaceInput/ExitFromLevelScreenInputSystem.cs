using Client.Data;
using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;

namespace Client
{
    public class ExitFromLevelScreenInputSystem : IEcsInitSystem
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
            _userInterfaceEventBus.ExitFromLevelScreen.ExitFromLevelButtonTap += () =>
            {
                _world.NewEntity().Get<DisposeLevelRequest>();
                _data.RuntimeData.CurrentGameState = GameState.GlobalMap;
                if (_data.InterstitialSettingsData.IsShowForExitLevelTap)
                    _adsService.ShowInter("level_exit");
                _analyticService.LogEvent("level_exit");
                _vibrationService.Vibrate(NiceHaptic.PresetType.LightImpact);
                _audioService.Play(Sounds.UiClickSound);
            };
        }
    }
}