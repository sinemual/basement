using Client.Data;
using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;

namespace Client
{
    public class GlobalMapScreenInputSystem : IEcsInitSystem
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
            _userInterfaceEventBus.GlobalMapScreen.StartNextLevelButton += () =>
            {
                _world.NewEntity().Get<DisposeLevelRequest>();
                _world.NewEntity().Get<SpawnLevelRequest>();
                _vibrationService.Vibrate(NiceHaptic.PresetType.LightImpact);
                _audioService.Play(Sounds.UiClickSound);
            };

            _userInterfaceEventBus.GlobalMapScreen.GoToVillageButton += () =>
            {
                _data.RuntimeData.CurrentGameState = !_data.PlayerData.TutrorialStates[TutorialStep.GoToVillage]
                    ? GameState.SceneBurningVillage
                    : GameState.Village;
                
                if (_data.InterstitialSettingsData.IsShowForGoToVillageTap)
                    _adsService.ShowInter("go_to_village");
                
                _vibrationService.Vibrate(NiceHaptic.PresetType.LightImpact);
                _audioService.Play(Sounds.UiClickSound);
            };
        }
    }
}