using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class PhysicAddExplosionForceSystem : IEcsRunSystem
    {
        private EcsFilter<AddExplosionForce> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var addingExplosionForce = ref entity.Get<AddExplosionForce>();

                Vector3 explosionPos = addingExplosionForce.Point;
                Collider[] results = new Collider[5];
                var size = Physics.OverlapSphereNonAlloc(explosionPos, addingExplosionForce.Radius, results);
                
                foreach (Collider hit in results)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                        rb.AddExplosionForce(addingExplosionForce.Power, explosionPos, addingExplosionForce.Radius, 50.0F);
                }
                
                entity.Del<AddExplosionForce>();
            }
        }
    }
}