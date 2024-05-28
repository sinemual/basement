using Client.Data.Core;
using Client.ECS.CurrentGame.Hit.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class ItemArcanoidSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private EcsFilter<PlayerProvider> _playerFilter;
        private EcsFilter<ThrowItem> _itemFilter;
        private EcsFilter<ThrowItem, OnTriggerEnterEvent> _filter;

        private Vector3 lastVelocity;
        public void Run()
        {
            foreach (var idx in _itemFilter)
            {
                ref var entity = ref _itemFilter.GetEntity(idx);
                ref var entityRb = ref entity.Get<RigidbodyProvider>().Value;
                lastVelocity = entityRb.velocity;
                
            }
            
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var evnt = ref entity.Get<OnTriggerEnterEvent>();
                ref var entityRb = ref entity.Get<RigidbodyProvider>().Value;
                ref var entityGo = ref entity.Get<GameObjectProvider>().Value;
                
                if (evnt.Collider.transform.TryGetComponent(out MonoEntity monoEntity))
                {
                    monoEntity.Entity.Get<HitRequest>().HitterEntity = _playerFilter.GetEntity(0);
                    entity.Get<HitRequest>().HitterEntity = monoEntity.Entity;

                    if (Physics.Raycast(entityGo.transform.position, lastVelocity.normalized, out var hit, 2.0f, _data.StaticData.GetBlocksLayer))
                    {
                        //var direction = entityGo.transform.position - evnt.Collision.transform.position;
                        var reflectDirection = Vector3.Reflect(lastVelocity.normalized, hit.normal);
                        Debug.Log("TUT");
                        Debug.DrawRay(evnt.Collider.ClosestPoint(entityGo.transform.position), reflectDirection, Color.red);
                        Debug.DrawRay(evnt.Collider.ClosestPoint(entityGo.transform.position), lastVelocity, Color.green);
                        entityRb.velocity = Vector3.zero;
                        entityRb.AddForce(reflectDirection * 10.0f, ForceMode.VelocityChange);
                    }
                }
            }
        }
    }
}