using Client.Data;
using Client.Data.Core;
using Leopotam.Ecs;

namespace Client.ECS.CurrentGame.Hit.Systems
{
    public class LocationInteractSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private CameraService _cameraService;

        private EcsFilter<LocationProvider, InteractRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var location = ref entity.Get<LocationProvider>();
                

                entity.Del<InteractRequest>();
            }
        }
    }
}