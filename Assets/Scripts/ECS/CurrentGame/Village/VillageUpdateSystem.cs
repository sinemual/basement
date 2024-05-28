using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Hit.Systems
{
    public class VillageUpdateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        
        private CameraService _cameraService;
        private UserInterfaceEventBus _uiEventBus;
        private GameUI _ui;
        private AnalyticService _analyticService;
        
        private EcsFilter<BuildingProvider> _filter;

        private EcsFilter<BuildEvent> _buildEventFilter;
        private EcsFilter<TimerDoneEvent<TimeToProduce>> _produceEventFilter;
        private EcsFilter<TakeProductItemsEvent> _takeProductEventFilter;
        private EcsFilter<GameStateChangedEvent> _gameStateChangedEventFilter;

        public void Init()
        {
            UpdateVillage();
        }

        public void Run()
        {
            if (_data.RuntimeData.CurrentGameState != GameState.Village)
                return;

            if (!_produceEventFilter.IsEmpty() || !_buildEventFilter.IsEmpty() || !_takeProductEventFilter.IsEmpty() ||
                !_gameStateChangedEventFilter.IsEmpty())
                UpdateVillage();
        }

        private void UpdateVillage()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var building = ref entity.Get<BuildingProvider>();
                ref var buildingGo = ref entity.Get<GameObjectProvider>().Value;

                DisabledAll(ref building);
                var buildingData = _data.PlayerData.BuildingsSaveData[building.Type];
                var buildingLevel = buildingData.CurrentLevel;

                building.OpenPanelButton.OnClickEvent.RemoveAllListeners();
                building.GetIncomeResourcesButton.OnClickEvent.RemoveAllListeners();

                var buildingType = building.Type;
                building.OpenPanelButton.OnClickEvent.AddListener( () => OpenBuildScreen(buildingType, buildingLevel + 1));
                building.LevelText.text = $"LEVEL {buildingLevel + 1}";
                
                var ecsEntity = entity;
                building.GetIncomeResourcesButton.OnClickEvent.AddListener( () => ecsEntity.Get<TakeProductItemsRequest>());
                
                building.OpenPanelButton.gameObject.SetActive(true);
                
                switch (buildingData.Status)
                {
                    case BuildingStatus.NotStarted:
                        building.BuidlingPlace.SetActive(true);
                        building.BuildPanel.SetActive(true);
                        building.LevelText.gameObject.SetActive(false);
                        break;
                    case BuildingStatus.Started:
                        /*building.TimerPanel.SetActive(true);
                        building.TimeText.text = */
                        break;
                    case BuildingStatus.Builded:
                        building.BuidlingByLevel[buildingLevel].SetActive(true);
                        building.LevelText.gameObject.SetActive(true);
                        
                        if (buildingLevel + 1 < _data.StaticData.BuildingsData[buildingType].Value.Count)
                            building.BuildPanel.SetActive(true);

                        if (_data.PlayerData.IsMetaTutorialComplete && building.Type == BuildingType.Lumberjack)
                            buildingGo.layer = _data.StaticData.GetRaycastLayer;

                        if (!entity.Has<Timer<TimeToProduce>>())
                        {
                            var produceTime = _data.StaticData.BuildingsData[building.Type].Value[buildingLevel].ProduceTimeInSec;
                            entity.Get<Timer<TimeToProduce>>().Value = produceTime;
                            building.IncomeProgressBarImage.DORewind();
                            building.IncomeProgressBarImage.fillAmount = 0;
                            building.IncomeProgressBarImage.DOFillAmount(1.0f, produceTime);
                        }
                           

                        int productItems = _data.StaticData.BuildingsData[building.Type].Value[buildingLevel].ProductionItem.Amount *
                                           _data.PlayerData.BuildingsSaveData[building.Type].IncomeTimes;

                        building.IncomePanel.SetActive(true);
                        building.IncomeResourcePanel.Image.sprite =
                            _data.StaticData.BuildingsData[building.Type].Value[buildingLevel].ProductionItem.ItemData.View.ItemSprite;
                        building.IncomeResourcePanel.AmountText.text = $"{productItems}";
                        break;
                }
            }
        }

        private void DisabledAll(ref BuildingProvider building)
        {
            building.BuidlingPlace.SetActive(false);
            foreach (var b in building.BuidlingByLevel)
                b.SetActive(false);

            building.OpenPanelButton.gameObject.SetActive(false);
            building.BuildPanel.SetActive(false);
            building.TimerPanel.SetActive(false);
            building.IncomePanel.SetActive(false);
        }

        private void OpenBuildScreen(BuildingType buildingType, int buildingLevel)
        {
            _uiEventBus.BuildScreen.OnUpdateScreen(_data.StaticData.BuildingsData[buildingType].Value[buildingLevel]);
            _ui.BuildScreen.SetShowState(true);
            _analyticService.LogEventWithParameter("pick_build_place", $"{buildingType}");
            _world.NewEntity().Get<PickBuildEvent>();
        }
        
        private void OpenUpgradeScreen(BuildingType buildingType, int buildingLevel)
        {
            if (buildingLevel < _data.StaticData.BuildingsData[buildingType].Value.Count)
            {
                _ui.BuildScreen.UpdateScreen(_data.StaticData.BuildingsData[buildingType].Value[buildingLevel]);
                _ui.BuildScreen.SetShowState(true);
                _analyticService.LogEventWithParameter("tap_upgrade_build", $"{buildingType}");
                _world.NewEntity().Get<PickBuildEvent>();
            }
        }
    }
}