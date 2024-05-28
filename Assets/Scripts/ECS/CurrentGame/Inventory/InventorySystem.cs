using Client.Data.Core;
using Client.ECS.CurrentGame.Loot.Systems;
using Leopotam.Ecs;

namespace Client.ECS.CurrentGame.PlayerEquipment
{
    public class InventorySystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;

        private EcsFilter<AddItemToInventoryRequest> _addFilter;
        private EcsFilter<RemoveItemFromInventoryRequest> _removeFilter;
        private EcsFilter<PlayerProvider> _playerFilter;

        public void Run()
        {
            foreach (var idx in _addFilter)
            {
                ref var item = ref _addFilter.Get1(idx).Value;

                if (_data.PlayerData.Inventory.ContainsKey(item.Id))
                {
                    _data.PlayerData.Inventory[item.Id] += 1;
                    if (_data.PlayerData.Inventory[item.Id] > 9)
                        _data.PlayerData.Inventory[item.Id] = 9;
                }
                else
                    _data.PlayerData.Inventory.Add(item.Id, 1);

                if (!_playerFilter.GetEntity(0).Has<HandItem>())
                    _playerFilter.GetEntity(0).Get<EquipHandItemRequest>().Value = item;
                else
                {
                    if (_playerFilter.GetEntity(0).Get<HandItem>().Data.Id == item.Id)
                        _ui.HandItemScreen.SetItem(item);
                }

                _addFilter.GetEntity(idx).Del<AddItemToInventoryRequest>();
            }

            foreach (var idx in _removeFilter)
            {
                ref var item = ref _removeFilter.Get1(idx).Value;

                if (_data.PlayerData.Inventory.ContainsKey(item.Id) && _data.PlayerData.Inventory[item.Id] > 0)
                    _data.PlayerData.Inventory[item.Id] -= 1;

                if (_data.PlayerData.Inventory[item.Id] <= 0)
                    _data.PlayerData.Inventory.Remove(item.Id);

                _removeFilter.GetEntity(idx).Del<RemoveItemFromInventoryRequest>();
            }
        }
    }
}