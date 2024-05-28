using Client.Data.Core;
using Client.Data.Equip;
using Client.DevTools.MyTools;
using Client.ECS.CurrentGame.PlayerEquipment.Components;
using Leopotam.Ecs;

namespace Client
{
    public class InventoryScreenInputSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private UserInterfaceEventBus _userInterfaceEventBus;
        
        private EcsFilter<PlayerProvider> _playerFilter;

        public void Init()
        {
            _userInterfaceEventBus.InventoryScreen.ChooseItemButtonTap += (data) =>
            {
                _playerFilter.GetEntity(0).Get<EquipItemRequest>().Value = data as EquipItemData;
            };
        }
    }
}