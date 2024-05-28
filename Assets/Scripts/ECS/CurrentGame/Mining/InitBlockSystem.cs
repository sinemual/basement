using Client.Data.Core;
using Client.Data.Equip;
using Data;
using Leopotam.Ecs;

namespace Client.ECS.CurrentGame.Mining
{
    public class InitBlockSystem : IEcsRunSystem
    {
        private SharedData _data;

        private EcsFilter<BlockProvider>.Exclude<InitedMarker> _blockFilter;

        public void Run()
        {
            foreach (var idx in _blockFilter)
            {
                ref var entity = ref _blockFilter.GetEntity(idx);
                ref var block = ref entity.Get<BlockProvider>();

                entity.Get<Stats>().Value = new StatValue();
                entity.Get<BaseStats>().Value = new StatValue();
                foreach (var stat in _data.StaticData.BlocksData[block.Type].Levels[block.Level].Stats)
                {
                    entity.Get<Stats>().Value.Add(stat.Key, stat.Value);
                    entity.Get<BaseStats>().Value.Add(stat.Key, stat.Value);
                }
                
                entity.Get<InitedMarker>();
            }
        }
    }
}