using Client.ECS.CurrentGame.Hit.Components;
using Leopotam.Ecs;

namespace Client.ECS.CurrentGame.Mining
{
    public class ExplosionEnemyTakeDamageSystem : IEcsRunSystem
    {
        private EcsFilter<EnemyProvider, ExplosionHitRequest> _requestfilter;

        public void Run()
        {
            foreach (var idx in _requestfilter)
            {
                ref var entity = ref _requestfilter.GetEntity(idx);
                ref var request = ref entity.Get<ExplosionHitRequest>();
                
                entity.Get<HitRequest>().HitterEntity = request.ExplosionSourceEntity;
                entity.Del<ExplosionHitRequest>();
            }
        }
    }
}