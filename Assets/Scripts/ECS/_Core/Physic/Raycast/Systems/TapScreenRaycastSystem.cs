using Client.Data;
using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class TapScreenRaycastSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;

        public void Run()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, 100f, _data.StaticData.RaycastMask))
                    _world.NewEntity().Get<RaycastEvent>() = new RaycastEvent
                    {
                        GameObject = hit.collider.gameObject,
                        HitPoint = hit.point
                    };
            }
        }
    }
}