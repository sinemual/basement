using Client.Data.Core;
using Client.ECS.CurrentGame.Experience;
using Leopotam.Ecs;

namespace Client.ECS.CurrentGame.Hit.Systems
{
    public class ShowRateUsPopupSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;
        private AnalyticService _analyticService;
        
        private EcsFilter<LevelCompleteEvent> _levelFilter;

        public void Run()
        {
            foreach (var idx in _levelFilter)
            {
                if (_data.RateUsData.NumberOfLevelToShowPopup.Contains(_data.PlayerData.EventLevelIndex))
                {
                    if (!_data.PlayerData.IsPlayerRatedGame)
                    {
                        _ui.RateUsScreen.SetShowState(true);
                        _analyticService.LogEvent("rate_us_popup_shown");
                    }
                }
            }
        }
    }
}