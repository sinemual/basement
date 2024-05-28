using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Loot.Systems;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;

namespace Client
{
    public class OpenChestScreenInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private AudioService _audioService;
        private AnalyticService _analyticService;
        private VibrationService _vibrationService;
        private UserInterfaceEventBus _userInterfaceEventBus;
        
        private EcsFilter<TimerDoneEvent<TimerToHideLootScreen>> _hideTimerFilter;
        
        public void Init()
        {
            _userInterfaceEventBus.OpenChestScreen.OpenChestButtonTap += (ItemData itemData) =>
            {
                if (itemData is ChestItemData chest)
                {
                    _audioService.Play(Sounds.ChestOpenSound);
                    _analyticService.LogEvent("chest_open");
                    _vibrationService.Vibrate(NiceHaptic.PresetType.LightImpact);
                    _world.NewEntity().Get<UiSpawnLootRequest>() = new UiSpawnLootRequest()
                    {
                        SpawnPosition = _ui.ChestScreen.GetFlyPoint().position,
                        Loot = chest.Loot
                    };
                    _world.NewEntity().Get<Timer<TimerToHideLootScreen>>().Value = 2.5f;
                }
            };
        }

        public void Run()
        {
            if(!_hideTimerFilter.IsEmpty() && _ui.ChestScreen.ScreenIsShow)
                _ui.ChestScreen.SetShowState(false);
        }
    }

    public struct TimerToHideLootScreen
    {
        public float Value;
    }
}