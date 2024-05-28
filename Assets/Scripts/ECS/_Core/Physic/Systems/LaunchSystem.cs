using Client.Data.Core;
using Client.DevTools.MyTools;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class LaunchSystem : IEcsRunSystem
    {
        private EcsFilter<RigidbodyProvider, LaunchRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var launchRequest = ref entity.Get<LaunchRequest>();
                ref var entityRb = ref entity.Get<RigidbodyProvider>();

                Utility.ResetRigibodyVelocity(entityRb.Value);

                var randomCircle = Random.insideUnitCircle;
                var randomDirection = new Vector3(randomCircle.x, 0.0f, randomCircle.y);
                var randomAngle = launchRequest.IsRandomDirection ? randomDirection : Vector3.zero;

                entity.Get<AddForce>() = new AddForce
                {
                    Direction = launchRequest.LaunchPoint.transform.up *
                        Random.Range(launchRequest.Force.x, launchRequest.Force.y) + randomAngle * 2.0f,
                    ForceMode = ForceMode.Impulse
                };

                entity.Get<LandingCheckRequest>();
                entity.Del<LaunchRequest>();
            }
        }
    }
}