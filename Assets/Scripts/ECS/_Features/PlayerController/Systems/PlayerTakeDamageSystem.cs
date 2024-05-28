using Client.Data;
using Client.Data.Core;
using Client.ECS.CurrentGame.Hit.Components;
using Client.ECS.CurrentGame.Loot.Systems;
using Client.Infrastructure.Services;
using Data;
using DG.Tweening;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;

namespace Client
{
    public class PlayerTakeDamageSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        
        private VibrationService _vibrationService;
        private CameraService _cameraService;
        private AudioService _audioService;
        private AnalyticService _analyticService;
        private UserInterfaceEventBus _uiEventBus;
        
        private EcsFilter<PlayerProvider, HitRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var stats = ref entity.Get<Stats>();
                ref var hitterStats = ref entity.Get<HitRequest>().HitterEntity.Get<Stats>();

                _vibrationService.Vibrate(NiceHaptic.PresetType.HeavyImpact);
                _audioService.Play(Sounds.BowHitSound);
                _cameraService.Shake();
                _analyticService.LogEvent("player_take_damage");
                
                stats.Value[StatType.Health] -= hitterStats.Value[StatType.Damage];
                
                _uiEventBus.PlayerDamage.OnPlayerDamageEvent(stats.Value[StatType.FullHealth],stats.Value[StatType.Health]);
                _uiEventBus.PlayerDamage.OnPlayerDamageFeelingEvent();
                
                if (stats.Value[StatType.Health] <= 0)
                    entity.Get<PlayerDeathEvent>();

                entity.Del<HitRequest>();
            }
        }
    }

    public struct PlayerDeathEvent : IEcsIgnoreInFilter
    {
    }
}