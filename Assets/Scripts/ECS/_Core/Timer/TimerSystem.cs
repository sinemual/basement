using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class TimerSystem<T> : IEcsRunSystem where T : struct
    {
        private EcsFilter<Timer<T>> _timerFilter;
        private EcsFilter<TimerDoneEvent<T>> _doneFilter;

        public void Run()
        {
            foreach (var idx in _doneFilter)
                _doneFilter.GetEntity(idx).Del<TimerDoneEvent<T>>();

            foreach (var idx in _timerFilter)
            {
                ref var timer = ref _timerFilter.Get1(idx);
                timer.Value -= Time.deltaTime;
                if (timer.Value <= 0)
                {
                    _timerFilter.GetEntity(idx).Get<TimerDoneEvent<T>>();
                    _timerFilter.GetEntity(idx).Del<Timer<T>>();
                }
            }
        }
    }
}