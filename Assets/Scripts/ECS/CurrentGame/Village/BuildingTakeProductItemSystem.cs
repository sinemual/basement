using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Loot.Systems;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Hit.Systems
{
    public class BuildingTakeProductItemSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private CameraService _cameraService;
        private AnalyticService _analyticService;
        private PrefabFactory _prefabFactory;
        
        private EcsFilter<BuildingProvider, TakeProductItemsRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var buildingProvider = ref entity.Get<BuildingProvider>();
                ref var buildingGo = ref entity.Get<GameObjectProvider>();

                var buildingSavedData = _data.PlayerData.BuildingsSaveData[buildingProvider.Type];
                var buildingLevel = buildingSavedData.CurrentLevel;
                
                int productItems = _data.StaticData.BuildingsData[buildingProvider.Type].Value[buildingLevel].ProductionItem.Amount *
                                   buildingSavedData.IncomeTimes;

                _data.PlayerData.BuildingsSaveData[buildingProvider.Type].IncomeTimes = 0;

                for (int i = 0; i < productItems; i++)
                {
                    GameObject spawnItem = _prefabFactory.SpawnGo(
                        _data.StaticData.BuildingsData[buildingProvider.Type].Value[buildingLevel].ProductionItem.ItemData.View.DropItemPrefab,
                        buildingGo.Value.transform.position + Vector3.one, Quaternion.identity);

                    _world.NewEntity().Get<GoToPlayerRequest>() = new GoToPlayerRequest()
                    {
                        ItemData = _data.StaticData.BuildingsData[buildingProvider.Type].Value[buildingLevel].ProductionItem.ItemData,
                        ItemGo = spawnItem
                    };
                }

                _analyticService.LogEventWithParameter("take_build_resources", $"{buildingProvider.Type}");
                entity.Del<TakeProductItemsRequest>();
                _world.NewEntity().Get<TakeProductItemsEvent>();
            }
        }
    }

    public struct TakeProductItemsEvent : IEcsIgnoreInFilter
    {
    }
}