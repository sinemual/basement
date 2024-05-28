using Client.Data.Core;
using Client.ECS.CurrentGame.Hit.Systems;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Mining
{
    public class UsedItemSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;

        private AudioService _audioService;

        private EcsFilter<ItemUsedEvent> _filter;
        private EcsFilter<PlayerProvider> _playerFilter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var item = ref entity.Get<ItemUsedEvent>().Value;

                _playerFilter.GetEntity(0).Del<DragHandItemState>();

                //Debug.Log($"{item.Id}");
                if (_data.PlayerData.Inventory.ContainsKey(item.Id))
                {
                    _data.PlayerData.CurrentHandItemId = item.Id;
                    _playerFilter.GetEntity(0).Get<HandItem>().Data = item;
                }
                else
                {
                    _data.PlayerData.CurrentHandItemId = "";
                    _playerFilter.GetEntity(0).Del<HandItem>();
                }
            }
        }
    }
}