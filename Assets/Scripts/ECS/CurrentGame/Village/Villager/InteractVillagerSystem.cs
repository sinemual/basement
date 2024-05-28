using Client.Data;
using Client.Data.Core;
using Leopotam.Ecs;

namespace Client.ECS.CurrentGame.Hit.Systems
{
    public class InteractVillagerSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private AudioService _audioService;

        private EcsFilter<VillagerProvider, InteractRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                _audioService.Play(Sounds.VillagerHmm);
                entity.Del<InteractRequest>();
            }
        }
    }
}