using Client.Data.Core;
using Client.ECS.CurrentGame.Hit.Systems;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Mining
{
    public class EnemyCheckPlayerSystem : IEcsRunSystem
    {
        private SharedData _data;

        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;
        private VibrationService _vibrationService;

        private EcsFilter<EnemyProvider>.Exclude<DeadState, Timer<ReloadingTimer>> _filter;
        private EcsFilter<EnemyProvider, ShootingState>.Exclude<DeadState, Timer<ReloadingTimer>> _shootFilter;
        private EcsFilter<MineEvent> _mineFilter;
        private EcsFilter<HitEvent> _hitFilter;
        private EcsFilter<PlayerProvider> _playerFilter;

        public void Run()
        {
            if (!_mineFilter.IsEmpty() || !_shootFilter.IsEmpty() || !_hitFilter.IsEmpty())
                foreach (var idx in _filter)
                {
                    ref var entity = ref _filter.GetEntity(idx);
                    ref var shooter = ref entity.Get<EnemyProvider>();
                    
                    if(!shooter.Head)
                        continue;
                    
                    GameObject playerGo = _playerFilter.GetEntity(0).Get<GameObjectProvider>().Value;
                    Vector3 direction = (playerGo.transform.position) - shooter.Head.transform.position;
                    Ray ray = new Ray(shooter.Head.transform.position, direction);
                    RaycastHit hit;
                    //Debug.DrawRay(shooter.Head.transform.position, (playerGo.transform.position), Color.red);

                    if (Physics.Raycast(ray, out hit, 100, _data.StaticData.RaycastMask))// Raycast
                    {
                        //Debug.Log($"Enemy ray hit: {hit.transform.gameObject}", hit.transform.gameObject);
                        if (hit.transform.TryGetComponent(out PlayerMonoProvider hitEntity))
                            entity.Get<AttackPlayerRequest>();
                    }
                }
        }
    }
}