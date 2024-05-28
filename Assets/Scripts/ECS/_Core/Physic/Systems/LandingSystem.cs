using Client.Data.Core;
using Leopotam.Ecs;

namespace Client
{
    public class LandingSystem : IEcsRunSystem
    {
        private SharedData _data;

        private EcsFilter<OnCollisionEnterEvent, LandingCheckRequest> _landingFilter;
        private EcsFilter<OnTriggerEnterEvent, LandingCheckRequest> _landingTriggerFilter;

        public void Run()
        {
            foreach (var landing in _landingFilter)
            {
                ref var landingEntity = ref _landingFilter.GetEntity(landing);
                ref var landingCollision = ref landingEntity.Get<OnCollisionEnterEvent>();

                if (landingCollision.Collision != null)
                    if (landingCollision.Collision.gameObject.CompareTag(_data.StaticData.GroundTag))
                    {
                        landingEntity.Del<LandingCheckRequest>();
                        landingEntity.Get<LandingEvent>();
                    }
            }

            foreach (var landing in _landingTriggerFilter)
            {
                ref var landingEntity = ref _landingTriggerFilter.GetEntity(landing);
                ref var landingCollision = ref landingEntity.Get<OnTriggerEnterEvent>();

                if (landingCollision.Collider)
                    if (landingCollision.Collider.CompareTag(_data.StaticData.GroundTag))
                    {
                        landingEntity.Del<LandingCheckRequest>();
                        landingEntity.Get<LandingEvent>();
                    }
            }
        }
    }
}