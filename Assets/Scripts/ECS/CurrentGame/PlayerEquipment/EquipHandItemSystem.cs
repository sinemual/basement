using Client.Data.Core;
using Client.ECS.CurrentGame.Loot.Systems;
using Leopotam.Ecs;

namespace Client.ECS.CurrentGame.PlayerEquipment
{
    public class EquipHandItemSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;

        private EcsFilter<EquipHandItemRequest> _filter;
        private EcsFilter<PlayerProvider> _playerFilter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var item = ref _filter.Get1(idx).Value;
                _data.PlayerData.CurrentHandItemId = item.Id;
                _playerFilter.GetEntity(0).Get<HandItem>().Data = item;
                _ui.HandItemScreen.SetItem(item);

                _filter.GetEntity(idx).Del<EquipHandItemRequest>();
            }
        }
    }
}