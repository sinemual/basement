using System;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Hit.Components;
using Client.ECS.CurrentGame.Hit.Systems;
using Client.Infrastructure.Services;
using Data;
using Leopotam.Ecs;

namespace Client.ECS.CurrentGame.Mining
{
    public class MiningSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private VibrationService _vibrationService;

        private EcsFilter<BlockProvider, HitRequest>.Exclude<MineEvent> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var stats = ref entity.Get<Stats>();
                ref var hitStats = ref entity.Get<HitRequest>().HitterEntity.Get<Stats>();

                stats.Value[StatType.Health] -= hitStats.Value[StatType.MiningDamage];

                if (stats.Value[StatType.Health] <= 0)
                    entity.Get<Timer<TimerToDisable>>().Value = 0.15f;

                entity.Get<MineEvent>();
                entity.Del<HitRequest>();
            }
        }
    }
}