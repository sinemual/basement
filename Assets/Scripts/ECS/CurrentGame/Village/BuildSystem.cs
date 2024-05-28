using Client.Client;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;

namespace Client
{
    public class BuildSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;
        
        private AudioService _audioService;
        private VibrationService _vibrationService;
        private AnalyticService _analyticService;
        
        private UserInterfaceEventBus _userInterfaceEventBus;

        private EcsFilter<BuildingProvider> _buildingsFilter;
        
        public void Init()
        {
            _userInterfaceEventBus.BuildScreen.BuildButtonTap += (BuildingData data) =>
            {
                _data.PlayerData.BuildingsSaveData[data.Type].CurrentLevel++;
                _data.PlayerData.BuildingsSaveData[data.Type].Status = BuildingStatus.Builded;
                _world.NewEntity().Get<BuildEvent>().Data = data;
                //_data.PlayerData.CraftedItem.Add(craftedItemData.GettedItem.Id);
                _audioService.Play(Sounds.Build);
                _vibrationService.Vibrate(NiceHaptic.PresetType.HeavyImpact);
                _analyticService.LogEventWithParameter("building_build", $"{data.Type}");
                
                foreach (var neededItem in data.NeededItems)
                    if (neededItem.ItemData is ResourceItemData res)
                    {
                        _world.NewEntity().Get<SpendResourceRequest>() = new SpendResourceRequest()
                        {
                            Type = res.Type,
                            Amount = neededItem.Amount
                        };
                    }

                foreach (var building in _buildingsFilter)
                    if(_buildingsFilter.Get1(building).Type == data.Type)
                        _buildingsFilter.Get1(building).BuildDustVFX.Play();
            };
            
            _userInterfaceEventBus.BuildScreen.UpgradeButtonTap += (BuildingData data) =>
            {
                _data.PlayerData.BuildingsSaveData[data.Type].CurrentLevel++;
                _data.PlayerData.BuildingsSaveData[data.Type].Status = BuildingStatus.Builded;
                _world.NewEntity().Get<BuildEvent>().Data = data;
                //_data.PlayerData.CraftedItem.Add(craftedItemData.GettedItem.Id);
                _audioService.Play(Sounds.Build);
                _vibrationService.Vibrate(NiceHaptic.PresetType.HeavyImpact);
                _analyticService.LogEventWithParameter("building_build", $"{data.Type}_{_data.PlayerData.BuildingsSaveData[data.Type].CurrentLevel}");
                
                foreach (var neededItem in data.NeededItems)
                    if (neededItem.ItemData is ResourceItemData res)
                    {
                        _world.NewEntity().Get<SpendResourceRequest>() = new SpendResourceRequest()
                        {
                            Type = res.Type,
                            Amount = neededItem.Amount
                        };
                    }

                foreach (var building in _buildingsFilter)
                    if(_buildingsFilter.Get1(building).Type == data.Type)
                        _buildingsFilter.Get1(building).BuildDustVFX.Play();
            };
        }
    }
}