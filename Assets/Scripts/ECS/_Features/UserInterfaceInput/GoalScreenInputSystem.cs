using Client.Data;
using Client.Data.Core;
using Client.ECS.CurrentGame.Loot.Systems;
using Client.Infrastructure.Services;
using Data;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace Client
{
    public class GoalScreenInputSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private AudioService _audioService;
        private AnalyticService _analyticService;
        private VibrationService _vibrationService;
        
        private UserInterfaceEventBus _userInterfaceEventBus;
        
        public void Init()
        {
            _userInterfaceEventBus.GameProgressScreen.TakeProgressRewardButtonTap += (GoalData goalData) =>
            {
                Debug.Log($"TakeProgressRewardButtonTap {goalData}");
                _analyticService.LogEventWithParameter("get_progress_goal_reward", $"{_data.StaticData.GameProgressGoals[_data.PlayerData.GameProgressStep].GoalDescriptionText}");
                _data.PlayerData.GameProgressData[_data.PlayerData.GameProgressStep].IsRewardTaken = true;
                _data.PlayerData.GameProgressStep += 1;
                    
                _audioService.Play(Sounds.PickupSound);
                _vibrationService.Vibrate(NiceHaptic.PresetType.LightImpact);
                    
                _world.NewEntity().Get<UiSpawnLootRequest>() = new UiSpawnLootRequest()
                {
                    SpawnPosition = _ui.GoalScreen.GetSpawnProgressRewardPoint().position,
                    Loot = goalData.Reward
                };
                _world.NewEntity().Get<TakeProgressRewardEvent>();
                _ui.GoalScreen.UpdateScreen();
            };
        }
    }

    public struct TakeProgressRewardEvent : IEcsIgnoreInFilter
    {
    }
}