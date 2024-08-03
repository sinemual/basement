using System;
using System.Linq;
using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Client
{
    public class SpawnLevelSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private PrefabFactory _prefabFactory;

        private EcsFilter<SpawnLevelRequest> _createNewLevelRequestFilter;
        private EcsFilter<SpawnRandomLevelRequest> _createRandomLevelRequestFilter;
        private EcsFilter<RestartLevelRequest> _restartLevelRequestFilter;

        public void Run()
        {
            foreach (var idx in _createNewLevelRequestFilter)
            {
                CreateNextLevel();
                _createNewLevelRequestFilter.GetEntity(idx).Del<SpawnLevelRequest>();
            }
            
            foreach (var idx in _createRandomLevelRequestFilter)
            {
                CreateRandomLevel(_createRandomLevelRequestFilter.Get1(idx).LocationType);
                _createRandomLevelRequestFilter.GetEntity(idx).Del<SpawnRandomLevelRequest>();
            }

            foreach (var idx in _restartLevelRequestFilter)
            {
                RestartLevel();
                _restartLevelRequestFilter.GetEntity(idx).Del<RestartLevelRequest>();
            }
        }

        private void CreateNextLevel()
        {
            Debug.Log($"CreateNextLevel");
            if (_data.PlayerData.EventLevelIndex > _data.StaticData.LevelsData.Levels.Count - 1)
            {
                _data.PlayerData.CurrentWarStepIndex = Random.Range(6, _data.StaticData.LevelsData.Levels.Count);
                    while (_data.PlayerData.CurrentWarStepIndex == _data.RuntimeData.LastLevelIndex)
                        _data.PlayerData.CurrentWarStepIndex = Random.Range(6, _data.StaticData.LevelsData.Levels.Count);
            }
            
            _data.RuntimeData.LastLevelIndex = _data.PlayerData.CurrentWarStepIndex;

            EcsEntity entity = _prefabFactory.Spawn(_data.StaticData.LevelsData.Levels[_data.PlayerData.CurrentWarStepIndex].gameObject, Vector3.zero,
                Quaternion.identity);

            _data.RuntimeData.CurrentLocationType = entity.Get<LevelProvider>().LocationType;
            
            entity.Get<CurrentLevelTag>();
            
            Debug.Log($"CreateLevel level {_data.PlayerData.CurrentWarStepIndex}");
        }

        private void RestartLevel()
        {
            _data.PlayerData.CurrentWarStepIndex = _data.RuntimeData.LastLevelIndex;
            CreateNextLevel();
        }

        private void CreateRandomLevel(LocationType location)
        {
            _data.RuntimeData.CurrentLocationType = location;

            _data.RuntimeData.LastLevelIndex = _data.PlayerData.CurrentWarStepIndex;
            Debug.Log($"CreateLevel level {_data.PlayerData.CurrentWarStepIndex}");

            EcsEntity entity = _prefabFactory.Spawn(_data.StaticData.LevelsData.RandomLevels[location].Value[_data.PlayerData.CurrentWarStepIndex].gameObject,
                Vector3.zero, Quaternion.identity);
            entity.Get<CurrentLevelTag>();
        }
    }

    internal struct SpawnRandomLevelRequest
    {
        public LocationType LocationType;
    }
}