using Client.Data;
using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Experience
{
    public class ExperienceSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;

        private UserInterfaceEventBus _uiEventBus;
        private AudioService _audioService;
        private AnalyticService _analyticService;

        private EcsFilter<GetExperienceRequest> _filter;

        public void Init()
        {
            _uiEventBus.Experience.OnGetExperience();
        }

        public void Run()
        {
            foreach (int index in _filter)
            {
                EcsEntity entity = _filter.GetEntity(index);
                int experience = entity.Get<GetExperienceRequest>().Value;

                _data.RuntimeData.CurrentLevelExperience += experience;
                
                if (!_data.RuntimeData.IsCurrentLevelCompleted)
                    if ((int)_data.RuntimeData.CurrentLevelExperience >= _data.RuntimeData.NeededLevelExperience)
                    {
                        _data.RuntimeData.IsCurrentLevelCompleted = true;
                        //_data.RuntimeData.CurrentGameState = GameState.LevelComplete;
                        _analyticService.LogEvent("level_complete");
                        _audioService.Play(Sounds.LevelComplete);
                        //_ui.LevelCompleteScreen.SetShowState(true);
                        _world.NewEntity().Get<LevelCompleteEvent>();
                    }
                
                if((int)_data.RuntimeData.CurrentLevelExperience >= _data.RuntimeData.AllLevelExperience)
                    _analyticService.LogEvent("total_level_complete");

                _uiEventBus.Experience.OnGetExperience();
                entity.Del<GetExperienceRequest>();
            }
        }
    }

    public struct LevelCompleteEvent
    {
    }
}