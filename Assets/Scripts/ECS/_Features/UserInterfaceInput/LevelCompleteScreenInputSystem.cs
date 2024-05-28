using Client.Client;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;

namespace Client
{
    public class LevelCompleteScreenInputSystem : IEcsInitSystem
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
            _userInterfaceEventBus.LevelCompleteScreen.GetRewardAndGoToNextLevelButtonTap += () =>
            {
#if UNITY_EDITOR
                GetReward();
#endif
                _adsService.ShowRewardVideo("get_x2_resources_for_level_complete", GetReward);
                _userInterfaceEventBus.LevelCompleteScreen.OnStartNextLevelButton();
            };

            _userInterfaceEventBus.LevelCompleteScreen.StartNextLevelButton += () =>
            {
                _data.PlayerData.CurrentLevelIndex++;
                _data.PlayerData.EventLevelIndex++;

                _vibrationService.Vibrate(NiceHaptic.PresetType.LightImpact);
                _audioService.Play(Sounds.UiClickSound);
                _world.NewEntity().Get<DisposeLevelRequest>();

                if (_data.PlayerData.CurrentLevelIndex % 5 == 0 && _data.PlayerData.CurrentLevelIndex > 11)
                    _data.RuntimeData.CurrentGameState = GameState.Village;
                else
                    _world.NewEntity().Get<SpawnLevelRequest>();

                if (_data.InterstitialSettingsData.IsShowForNextLevelTap)
                    _adsService.ShowInter("next_level");
            };

            _userInterfaceEventBus.LevelCompleteScreen.BackToMetaButton += () =>
            {
                _data.PlayerData.CurrentLevelIndex++;
                _data.PlayerData.EventLevelIndex++;
                
                _world.NewEntity().Get<DisposeLevelRequest>();
                _data.RuntimeData.CurrentGameState = GameState.GlobalMap;
                _vibrationService.Vibrate(NiceHaptic.PresetType.LightImpact);
                _audioService.Play(Sounds.UiClickSound);
                
                if (_data.InterstitialSettingsData.IsShowForGoToGlobalMapTap)
                    _adsService.ShowInter("go_to_global_map");
            };
        }

        private void GetReward()
        {
            for (int i = 0; i < _data.RuntimeData.MinedLevelResources.Count; i++)
            {
                _world.NewEntity().Get<AddResourceRequest>() = new AddResourceRequest()
                {
                    Amount = _data.RuntimeData.MinedLevelResources[(ResourceType)i],
                    Type = (ResourceType)i
                };
            }
        }
    }
}