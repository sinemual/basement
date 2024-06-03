using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Experience;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class CheckProgressGoalCompleteSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;

        private UserInterfaceEventBus _uiEventBus;
        private AnalyticService _analyticService;

        private EcsFilter<BuildingProvider> _buildFilter;

        private EcsFilter<BuildEvent> _buildEventFilter;
        private EcsFilter<GameStateChangedEvent> _gameStateChangedFilter;
        private EcsFilter<TakeProgressRewardEvent> _takeProgressRewardEventFilter;
        private EcsFilter<LevelCompleteEvent> _levelCompleteEventFilter;
        private EcsFilter<CheckGoalStateEvent> _checkEventFilter;

        public void Run()
        {
            if (!_buildEventFilter.IsEmpty() || !_gameStateChangedFilter.IsEmpty() || !_takeProgressRewardEventFilter.IsEmpty()
                || !_levelCompleteEventFilter.IsEmpty() || !_checkEventFilter.IsEmpty())
            {
                if (_data.PlayerData.GameProgressStep < _data.StaticData.GameProgressGoals.Count)
                {
                    var goalData = _data.StaticData.GameProgressGoals[_data.PlayerData.GameProgressStep];
                    if (goalData.Type == GoalType.Build)
                        foreach (var build in _buildFilter)
                        {
                            if (_buildFilter.Get1(build).Type == goalData.BuildingType &&
                                _data.PlayerData.BuildingsSaveData[goalData.BuildingType].Status == BuildingStatus.Builded)
                            {
                                var playerGoalData = _data.PlayerData.GameProgressData[_data.PlayerData.GameProgressStep];
                                playerGoalData.CurrentValue++;
                                if (playerGoalData.CurrentValue >= goalData.GoalValue)
                                {
                                    _analyticService.LogEventWithParameter("progress_goal_complete", goalData.GoalDescriptionText);
                                    _data.PlayerData.GameProgressData[_data.PlayerData.GameProgressStep].IsCompleted = true;
                                    //_ui.GameProgressScreen.UpdateScreen();
                                    _world.NewEntity().Get<ProgressGoalCompleteEvent>().Type = goalData.Type;
                                }
                            }
                        }

                    if (goalData.Type == GoalType.GetLevel)
                    {
                        var playerGoalData = _data.PlayerData.GameProgressData[_data.PlayerData.GameProgressStep];
                        playerGoalData.CurrentValue = _data.PlayerData.EventLevelIndex;
                        if (playerGoalData.CurrentValue + 1 >= goalData.GoalValue)
                        {
                            _analyticService.LogEventWithParameter("progress_goal_complete", goalData.GoalDescriptionText);
                            _data.PlayerData.GameProgressData[_data.PlayerData.GameProgressStep].IsCompleted = true;
                            _world.NewEntity().Get<ProgressGoalCompleteEvent>().Type = goalData.Type;
                            _ui.GoalScreen.UpdateScreen();
                            
                        }
                    }
                }
            }
        }
    }

    internal struct CheckGoalStateEvent
    {
    }
}