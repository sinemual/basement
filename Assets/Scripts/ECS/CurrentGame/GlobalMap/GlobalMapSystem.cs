using System;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.Infrastructure.Services;
using DG.Tweening;
using EPOOutline;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Hit.Systems
{
    public class GlobalMapSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private CameraService _cameraService;
        private AnalyticService _analyticService;

        private EcsFilter<LocationProvider, InitedMarker> _filter;
        private EcsFilter<LocationProvider>.Exclude<InitedMarker> _notInitedfilter;
        private EcsFilter<GlobalMapProvider> _globalMapFilter;

        private EcsFilter<GameStateChangedEvent> _gameStateEventFilter;

        public void Run()
        {
            if (_gameStateEventFilter.IsEmpty())
                return;

            if (_data.RuntimeData.CurrentGameState != GameState.GlobalMap)
                return;

            foreach (var idx in _globalMapFilter)
            {
                ref var entity = ref _globalMapFilter.GetEntity(idx);
                var globalMapProvider = entity.Get<GlobalMapProvider>();

                if (_data.PlayerData.EventLevelIndex <= _data.StaticData.LevelsData.Levels.Count - 1)
                {
                    /*var position = _cameraService.CameraSceneData.DollyCart.GetClosestPoint(
                        globalMapProvider.LevelPoints[_data.PlayerData.EventLevelIndex].transform.position,
                        _cameraService.CameraSceneData.DollyCart.StartPoint.position, _cameraService.CameraSceneData.DollyCart.EndPoint.position);
                    */
                    var positionZ = globalMapProvider.LevelPoints[_data.PlayerData.EventLevelIndex].transform.position.z;
                    var positionX = globalMapProvider.LevelPoints[_data.PlayerData.EventLevelIndex].transform.position.x + 10.75f;
                    positionZ -= 5.5f + positionX * 0.8f;
                    globalMapProvider.CurrentPointParticle.transform.position =
                        globalMapProvider.LevelPoints[_data.PlayerData.EventLevelIndex].transform.position;
                }
                else
                {
                    globalMapProvider.CurrentPointParticle.transform.position =
                        globalMapProvider.LevelPoints[_data.StaticData.LevelsData.Levels.Count - 1].transform.position;
                }

                for (int i = 0; i < globalMapProvider.Locations.Count; i++)
                {
                    globalMapProvider.Locations[(LocationType)i].material = _data.StaticData.ClosedLocationMaterial;
                    //globalMapProvider.Locations[(LocationType)i].GetComponent<Outlinable>().enabled = false;
                }
            }

            /*foreach (var idx in _notInitedfilter)
            {
                ref var entity = ref _notInitedfilter.GetEntity(idx);
                var location = entity.Get<LocationProvider>();

                /*location.StartButton.OnClickEvent.AddListener(() =>
                {
                    _world.NewEntity().Get<SpawnRandomLevelRequest>().LocationType = location.LocationType;
                    _analyticService.LogEventWithParameter("go_to_random_level_location", $"{location.LocationType}");
                });#1#
                entity.Get<InitedMarker>();
            }*/
        }
    }
}