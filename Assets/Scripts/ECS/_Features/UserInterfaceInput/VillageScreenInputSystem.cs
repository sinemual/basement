using Client.Data;
using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;

namespace Client
{
    public class VillageScreenInputSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private AudioService _audioService;
        private UserInterfaceEventBus _userInterfaceEventBus;
        private VibrationService _vibrationService;
        private AdsService _adsService;

        public void Init()
        {
            _userInterfaceEventBus.VillageScreen.GoToGlobalMapButtonTap += () =>
            {
                _data.RuntimeData.CurrentGameState = GameState.GlobalMap;
                if (_data.InterstitialSettingsData.IsShowForGoToGlobalMapTap)
                    _adsService.ShowInter("go_to_global_map");
                _vibrationService.Vibrate(NiceHaptic.PresetType.LightImpact);
                _audioService.Play(Sounds.UiClickSound);
            };
        }
    }
}