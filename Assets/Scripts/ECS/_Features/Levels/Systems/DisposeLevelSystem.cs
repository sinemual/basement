using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;
using CameraType = Client.Data.CameraType;

namespace Client
{
    public class DisposeLevelSystem : IEcsRunSystem
    {
        private SharedData _data;
        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;

        //private EcsFilter<SpawnLevelRequest> _spawnRequestFilter;
        private EcsFilter<RestartLevelRequest> _restartRequestFilter;
        private EcsFilter<DisposeLevelRequest> _disposeRequestFilter;

        private EcsFilter<CurrentLevelTag> _currentLevelFilter;
        private EcsFilter<FromThisLevelTag> _fromThisLevelFilter;

        public void Run()
        {
            if (!_currentLevelFilter.IsEmpty() && (!_restartRequestFilter.IsEmpty() || !_disposeRequestFilter.IsEmpty()))
                DespawnCurrentLevel();

            foreach (var req in _disposeRequestFilter)
                _disposeRequestFilter.GetEntity(req).Del<DisposeLevelRequest>();
        }

        private void DespawnCurrentLevel()
        {
            _cameraService.GetVCByType(CameraType.LevelCamera).transform.SetParent(null);

            foreach (var fromThisLevel in _fromThisLevelFilter)
                _prefabFactory.Despawn(ref _fromThisLevelFilter.GetEntity(fromThisLevel));

            foreach (var fromThisLevel in _fromThisLevelFilter)
                _prefabFactory.Despawn(ref _fromThisLevelFilter.GetEntity(fromThisLevel));

            foreach (var currentLevel in _currentLevelFilter)
                _prefabFactory.Despawn(ref _currentLevelFilter.GetEntity(currentLevel));
        }
    }

    internal struct DisposeLevelRequest : IEcsIgnoreInFilter
    {
    }
}