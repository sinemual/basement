using Client.Data.Core;
using Client.Data.Equip;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;

namespace Client
{
    public class GetItemScreenInputSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private UserInterfaceEventBus _userInterfaceEventBus;
        private VibrationService _vibrationService;
        
        public void Init()
        {
            _userInterfaceEventBus.GetItemScreen.GetItemButtonTap += (ItemData itemData) =>
            {
                _vibrationService.Vibrate(NiceHaptic.PresetType.LightImpact);
            };
        }
    }
}