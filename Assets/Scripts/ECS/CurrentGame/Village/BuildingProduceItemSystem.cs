using System;
using Client.Data.Core;
using Client.Data.Equip;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Hit.Systems
{
    public class BuildingProduceItemSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private CameraService _cameraService;

        private EcsFilter<BuildingProvider, TimerDoneEvent<TimeToProduce>> _filter;
        private EcsFilter<BuildingProvider> _buildingFilter;

        public void Init()
        {
            foreach (var idx in _buildingFilter)
            {
                var buildingProvider = _buildingFilter.Get1(idx);
                var buildingSavedData = _data.PlayerData.BuildingsSaveData[buildingProvider.Type];
                var buildingLevel = buildingSavedData.CurrentLevel;
                
                if (buildingSavedData.Status == BuildingStatus.Builded)
                    _buildingFilter.GetEntity(idx).Get<Timer<TimeToProduce>>().Value =
                        _data.StaticData.BuildingsData[buildingProvider.Type].Value[buildingLevel].ProduceTimeInSec;
            }
                
        }

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var buildingProvider = ref entity.Get<BuildingProvider>();

                var buildingSavedData = _data.PlayerData.BuildingsSaveData[buildingProvider.Type];
                var buildingLevel = buildingSavedData.CurrentLevel;
                
                buildingSavedData.IncomeTimes += 1;
                buildingProvider.IncomePanel.gameObject.transform.DORewind();
                buildingProvider.IncomePanel.gameObject.transform.DOPunchScale(Vector3.one * 0.1f, 0.15f, 2, 0.5f);
                buildingProvider.IncomeProgressBarImage.DORewind();
                buildingProvider.IncomeProgressBarImage.fillAmount = 0;
                var produceTime = _data.StaticData.BuildingsData[buildingProvider.Type].Value[buildingLevel].ProduceTimeInSec;
                buildingProvider.IncomeProgressBarImage.DOFillAmount(1.0f, produceTime);
                entity.Get<Timer<TimeToProduce>>().Value = produceTime;
            }
        }
    }
}