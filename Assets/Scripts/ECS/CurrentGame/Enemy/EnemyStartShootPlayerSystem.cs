using Client.Data;
using Client.Data.Core;
using Client.ECS.CurrentGame.Hit.Systems;
using Data;
using DG.Tweening;
using Leopotam.Ecs;

namespace Client.ECS.CurrentGame.Mining
{
    public class EnemyStartShootPlayerSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;

        private EcsFilter<EnemyShootProvider, AttackPlayerRequest> _requestfilter;
        private EcsFilter<PlayerProvider> _playerFilter;

        public void Run()
        {
            foreach (var idx in _requestfilter)
            {
                ref var entity = ref _requestfilter.GetEntity(idx);
                ref var animator = ref entity.Get<AnimatorProvider>();
                ref var entityStats = ref entity.Get<Stats>();
                ref var enemyGo = ref entity.Get<GameObjectProvider>().Value;
                
                animator.Value.SetTrigger(Animations.IsShoot);
                
                var playerPos = _playerFilter.GetEntity(0).Get<GameObjectProvider>().Value.transform.position;
                
                enemyGo.transform.DOLookAt(playerPos, 0.5f, AxisConstraint.Y);
                
                entity.Get<ShootingState>();
                //entity.Get<ShootRequest>();
                entity.Get<Timer<ReloadingTimer>>().Value = 5.0f * entityStats.Value[StatType.AttackSpeed];
                entity.Del<AttackPlayerRequest>();
            }
        }
    }
}