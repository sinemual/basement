using Leopotam.Ecs;

namespace Client
{
    public class PhysicForceAddSystem : IEcsRunSystem
    {
        private EcsFilter<AddForce, RigidbodyProvider> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var entityRb = ref entity.Get<RigidbodyProvider>();
                ref var force = ref entity.Get<AddForce>();

                entityRb.Value.AddForce(force.Direction, force.ForceMode);

                entity.Del<AddForce>();
            }
        }
    }
}