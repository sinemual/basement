using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Mining;
using Client.Infrastructure.Services;
using Leopotam.Ecs;

namespace Client
{
    public class CatchEventSystem : IEcsRunSystem
    {
        private AnalyticService _analyticService;
        
        private EcsFilter<MinedEvent> _mineFilter;
        private EcsFilter<DeadEvent> _killFilter;
        private EcsFilter<ItemCraftedEvent> _craftFilter;
        private EcsFilter<ItemUsedEvent> _useFilter;
        private EcsFilter<ChestFoundEvent> _chestFilter;
        private EcsFilter<LevelGoalCompleteEvent> _levelGoalCompleteFilter;

        public void Run()
        {
            foreach (var evnt in _mineFilter)
                _analyticService.LogEventWithParameter("res_mined", _mineFilter.Get1(evnt).BlockType.ToString());

            foreach (var evnt in _killFilter)
                _analyticService.LogEventWithParameter("enemy_killed", $"{_killFilter.Get1(evnt).CharacterType}");

            foreach (var evnt in _craftFilter)
                _analyticService.LogEventWithParameter($"tool_{_craftFilter.Get1(evnt).Value.Id}_upgrade", $"{_craftFilter.Get1(evnt).Value.Level}");

            foreach (var evnt in _useFilter)
                _analyticService.LogEventWithParameter("item_used", $"{_useFilter.Get1(evnt).Value.Id}");

            foreach (var evnt in _chestFilter)
                _analyticService.LogEvent("chest_found");
            
            foreach (var evnt in _levelGoalCompleteFilter)
                _analyticService.LogEventWithParameter("level_goal_complete", $"{_levelGoalCompleteFilter.Get1(evnt).Type}");
        }
    }
}