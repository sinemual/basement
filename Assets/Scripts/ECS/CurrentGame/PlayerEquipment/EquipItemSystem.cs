using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.PlayerEquipment.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.PlayerEquipment
{
    public class EquipItemSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;

        private EcsFilter<EquipItemRequest> _filter;
        private EcsFilter<PlayerProvider> _playerFilter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var item = ref _filter.Get1(idx).Value;
                
                _data.PlayerData.Equipment[item.Type] = item.Level;
                _playerFilter.GetEntity(0).Get<Equipment>().Value[item.Type] = item;
                _playerFilter.GetEntity(0).Get<RecalculateStatsRequest>();
                
                _filter.GetEntity(idx).Del<EquipItemRequest>();
            }
        }
    }
}