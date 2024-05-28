using Client.Data.Core;
using Client.ECS.CurrentGame.Experience;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Hit.Systems
{
    public class OpenLocationGlobalMapSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private EcsFilter<LevelCompleteEvent> _levelFilter;

        public void Run()
        {
            foreach (var idx in _levelFilter)
            {
                Debug.Log($"_data.RuntimeData.CurrentLocationType {_data.RuntimeData.CurrentLocationType}");

                if (_data.PlayerData.EventLevelIndex + 1 <= _data.StaticData.LevelsData.Levels.Count - 1 &&
                    _data.RuntimeData.CurrentLocationType != _data.StaticData.LevelsData.Levels[_data.PlayerData.EventLevelIndex + 1]
                        .GetComponent<LevelMonoProvider>().Value.LocationType)
                    _data.PlayerData.OpenLocations[_data.StaticData.LevelsData.Levels[_data.PlayerData.EventLevelIndex + 1]
                        .GetComponent<LevelMonoProvider>().Value.LocationType] = true;
            }
        }
    }
}