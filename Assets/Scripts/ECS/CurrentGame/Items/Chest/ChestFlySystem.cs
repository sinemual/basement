using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Mining
{
    public class ChestFlySystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;

        private EcsFilter<ChestProvider, RigidbodyProvider>.Exclude<InitedMarker> _initFilter;
        private EcsFilter<ChestProvider, RigidbodyProvider, InitedMarker, TimerDoneEvent<TimerToFly>> _flyFilter;

        public void Run()
        {
            foreach (var idx in _initFilter)
            {
                ref var entity = ref _initFilter.GetEntity(idx);
                var rb = entity.Get<RigidbodyProvider>().Value;
                var go = entity.Get<GameObjectProvider>().Value;

                if (_data.BalanceData.FlyRandomChance > Random.Range(0.0f, 1.0f))
                {
                    go.transform.position = new Vector3(go.transform.position.x, 25.0f, go.transform.position.z);
                    rb.isKinematic = true;
                    entity.Get<Timer<TimerToFly>>().Value = Random.Range(_data.BalanceData.StartFlyRandomTime, _data.BalanceData.EndFlyRandomTime);
                }
                else
                {
                    rb.isKinematic = false;
                }

                entity.Get<InitedMarker>();
            }

            foreach (var idx in _flyFilter)
            {
                ref var entity = ref _flyFilter.GetEntity(idx);
                var rb = entity.Get<RigidbodyProvider>().Value;

                rb.isKinematic = false;
            }
        }
    }
}