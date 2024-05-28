using Leopotam.Ecs;

namespace Client
{
    public class PhysicForceAddToPointSystem : IEcsRunSystem
    {
        private EcsFilter<AddForceAtPoint, RigidbodyProvider> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var entityRb = ref entity.Get<RigidbodyProvider>();
                ref var force = ref entity.Get<AddForceAtPoint>();

                entityRb.Value.AddForceAtPosition(force.Direction, force.Point, force.ForceMode);

                entity.Del<AddForce>();
            }
        }
    }
}