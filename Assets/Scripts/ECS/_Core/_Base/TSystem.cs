using System;
using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class TSystem : IEcsInitSystem, IEcsRunSystem
    {
        //private GameData _gameData;
        //private GameUI _ui;
        //private WorldGameUI _worldGameUI;
        //private SceneObjectsContainer _sceneObjectsContainer;
        //private EcsWorld _world;

        //private AnalyticService _analyticService;
        //private CameraService _cameraService;

        //private EcsFilter<SetGameStateRequest>.Exclude<Timer<TimerInteraction>, InteractionDoneMarker> _filter;

        public void Init()
        {
            //throw new NotImplementedException();
        }

        public void Run()
        {
            //foreach (var idx in _filter)
            //{
            //    ref var entity = ref _filter.GetEntity(idx);
            //    ref var createEarnViewRequest = ref entity.Get<CreateEarnViewRequest>();
            //    entity.Get<PoolObject>();
            //    entity.Get<SpawnPrefab>() = new SpawnPrefab
            //    {
            //        Prefab = _gameData.StaticData.EarnInfoPrefab,
            //        Position = createEarnViewRequest.SpawnPoint.transform.position + Vector3.up,
            //       Rotation = Quaternion.identity,
            //        Parent = null,
            //        Entity = entity
            //    };
            //    entity.Get<PoolObjectRequest>();
            //}
        }
    }
}