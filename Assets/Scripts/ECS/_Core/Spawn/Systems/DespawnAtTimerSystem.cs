using Client.Data.Core;
using Client.DevTools.MyTools;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class DespawnAtTimerSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;

        private PrefabFactory _prefabFactory;

        private EcsFilter<DespawnAtTimerRequest, TimerDoneEvent<DespawnTimer>> _timerFilter;

        public void Run()
        {
            foreach (var idx in _timerFilter)
                _prefabFactory.Despawn(ref _timerFilter.GetEntity(idx));
        }
    }

    internal struct DespawnTimer
    {
        public float Value;
    }
}