using Client.Data;
using Client.Data.Core;
using Client.ECS.CurrentGame.PlayerEquipment.Components;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace Client.ECS.CurrentGame.Mining
{
    public class DurabilitySystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private PrefabFactory _prefabFactory;
        private VibrationService _vibrationService;
        private AudioService _audioService;

        //private EcsFilter<SetItemDurabilityRequest> _filter;
        private EcsFilter<PlayerProvider> _playerFilter;
        

        public void Run()
        {
            /*foreach (var idx in _filter)
            {
                ref var requestEntity = ref _filter.GetEntity(idx);
                ref var request = ref requestEntity.Get<SetItemDurabilityRequest>();

                ref var playerEntity = ref _playerFilter.GetEntity(0);
                ref var equip = ref playerEntity.Get<Equipment>();
                
                if (equip.Value.ContainsKey(request.Type))
                {
                    equip.Value[request.Type].Stats[StatType.Health] -= _data.BalanceData.DurabilityLossCoef;
                    _data.PlayerData.SavedEquipDurability[request.Type] = equip.Value[request.Type].Stats[StatType.Health];
                
                    float durabilityAmount = equip.Value[request.Type].Stats[StatType.Health] / equip.Value[request.Type].Stats[StatType.StartHealth];

                    _ui.EquipScreen.SetItemDurability(request.Type, durabilityAmount);
                    
                    if (equip.Value[request.Type].Stats[StatType.Health] <= 0 && _data.PlayerData.SavedEquip.ContainsKey(request.Type))
                    {
                        _data.PlayerData.SavedEquip.Remove(request.Type);
                        _data.PlayerData.SavedEquipDurability.Remove(request.Type);
                        _playerFilter.GetEntity(0).Get<Equipment>().Value.Remove(request.Type);
                        _playerFilter.GetEntity(0).Get<RecalculateStatsRequest>();
                        _ui.EquipScreen.DelItem(request.Type);
                        _vibrationService.Vibrate(NiceHaptic.PresetType.HeavyImpact);
                        _audioService.Play(Sounds.ItemBrokenSound);
                    }
                }
                requestEntity.Del<SetItemDurabilityRequest>();
            }*/
        }
    }
}