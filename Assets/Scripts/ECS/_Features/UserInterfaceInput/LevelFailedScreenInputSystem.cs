using Client.Data.Core;
using Client.DevTools.MyTools;
using Data;
using Leopotam.Ecs;

namespace Client
{
    public class LevelFailedScreenInputSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private UserInterfaceEventBus _uiEventBus;
        private AdsService _adsService;
        private AnalyticService _analyticService;
        
        private EcsFilter<PlayerProvider> _playerFilter;
        
        public void Init()
        {
            _uiEventBus.LevelFailedScreen.RestartLevelButtonTap += () => { _world.NewEntityWith<RestartLevelRequest>(); };

            _uiEventBus.LevelFailedScreen.RewardReviveButtonTap += () =>
            {
#if UNITY_EDITOR
                GetReward();
#endif
                _adsService.ShowRewardVideo("revive", GetReward);
            };
        }

        private void GetReward()
        {
            _playerFilter.GetEntity(0).Get<Stats>().Value[StatType.Health] = _playerFilter.GetEntity(0).Get<Stats>().Value[StatType.FullHealth];
            _uiEventBus.PlayerDamage.OnInitPlayerHealthEvent(
                _playerFilter.GetEntity(0).Get<Stats>().Value[StatType.FullHealth],
                _playerFilter.GetEntity(0).Get<Stats>().Value[StatType.Health]);
        }
    }
}