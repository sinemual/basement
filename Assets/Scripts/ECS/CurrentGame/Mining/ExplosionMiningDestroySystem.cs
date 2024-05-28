using Client.Data.Core;
using Data;
using Leopotam.Ecs;

namespace Client.ECS.CurrentGame.Mining
{
    public class ExplosionMiningDestroySystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private PrefabFactory _prefabFactory;

        private EcsFilter<BlockProvider, ExplosionHitRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var stats = ref entity.Get<Stats>();
                ref var hitStats = ref entity.Get<ExplosionHitRequest>().ExplosionSourceEntity.Get<Stats>();

                stats.Value[StatType.Health] -= hitStats.Value[StatType.MiningDamage];

                if (stats.Value[StatType.Health] <= 0)
                    entity.Get<Timer<TimerToDisable>>().Value = 0.15f;
                
                
                entity.Del<ExplosionHitRequest>();
            }
        }
    }
}