using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class ThrowSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private EcsFilter<ThrowTrajectoryProvider, ThrowRequest> _requestFilter;
        //private EcsFilter<ThrowItem, ReadyMarker> _itemFilter;

        public void Run()
        {
            foreach (var request in _requestFilter)
            {
                ref var entity = ref _requestFilter.GetEntity(request);
                ref var startPoint = ref entity.Get<ThrowTrajectoryProvider>().StartPoint;
                
                /*foreach (var item in _itemFilter)
                {
                    ref var itemEntity = ref _itemFilter.GetEntity(item);
                    ref var itemRb = ref itemEntity.Get<RigidbodyProvider>().Value;
                    ref var itemGo = ref itemEntity.Get<GameObjectProvider>().Value;
                    itemGo.transform.SetParent(null);
                    itemRb.isKinematic = false;
                    itemRb.AddForce(startPoint.forward * 10.0f, ForceMode.VelocityChange);
                    itemEntity.Del<ReadyMarker>();
                }*/
                entity.Del<ThrowRequest>();
            }
        }
    }
}