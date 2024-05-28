using Client.Data;
using Client.Data.Core;
using Leopotam.Ecs;

namespace Client
{
    public class SettingScreenInputSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private AudioService _audioService;
        
        private UserInterfaceEventBus _userInterfaceEventBus;

        public void Init()
        {
            _userInterfaceEventBus.SettingScreen.VibrationTriggerButtonTap += () =>
            {
                _data.PlayerData.IsVibrationOn = !_data.PlayerData.IsVibrationOn;
                _ui.SettingScreen.UpdateSprites();
                _audioService.Play(Sounds.UiClickSound);
            };
            
            _userInterfaceEventBus.SettingScreen.SoundTriggerButtonTap += () =>
            {
                _data.PlayerData.IsSoundOn = !_data.PlayerData.IsSoundOn;
                _audioService.ToggleAudio(_data.PlayerData.IsSoundOn);
                _ui.SettingScreen.UpdateSprites();
                _audioService.Play(Sounds.UiClickSound);
            };
        }
    }
}