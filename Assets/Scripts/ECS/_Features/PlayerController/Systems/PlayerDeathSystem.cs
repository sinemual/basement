using Client.Data;
using Client.Data.Core;
using Client.ECS.CurrentGame.Mining;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;

namespace Client
{
    public class PlayerDeathSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        
        private VibrationService _vibrationService;
        private CameraService _cameraService;
        private AudioService _audioService;
        private AnalyticService _analyticService;
        
        private EcsFilter<PlayerDeathEvent> _filter;
        private EcsFilter<CharacterProvider> _shootersFilter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);

                _vibrationService.Vibrate(NiceHaptic.PresetType.HeavyImpact);
                _audioService.Play(Sounds.BowHitSound);
                _cameraService.Shake();
                _analyticService.LogEvent("player_death");
                
               // _ui.LevelFailedScreen.SetShowState(true);

                foreach (var shooter in _shootersFilter)
                {
                    _shootersFilter.GetEntity(shooter).Del<ShootingState>();
                }
            }
        }
    }
}