using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class RaycastSystem : IEcsRunSystem
    {
        private EcsFilter<RaycastProvider> _filter;
        
        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var raycastProvider = ref entity.Get<RaycastProvider>();
                
                if (Physics.Raycast(raycastProvider.StartRaycastPoint.position, raycastProvider.StartRaycastPoint.TransformDirection(Vector3.forward), out var hit, raycastProvider.RaycastLength))
                    entity.Get<RaycastEvent>() = new RaycastEvent
                    {
                        GameObject = hit.collider.gameObject,
                        HitPoint = hit.point
                    };
            }
        }
    }
}