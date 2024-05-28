using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Loot.Systems;
using Leopotam.Ecs;

namespace Client.ECS.CurrentGame.PlayerEquipment
{
    public class AddToInventoryCraftedItemSystem : IEcsRunSystem
    {
        private SharedData _data;

        private EcsFilter<ItemCraftedEvent> _filter;
        private EcsFilter<PlayerProvider> _playerFilter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var craftedItem = ref _filter.Get1(idx).Value;
                if (craftedItem is IUsable)
                    _playerFilter.GetEntity(0).Get<AddItemToInventoryRequest>().Value = craftedItem;
            }
        }
    }
}