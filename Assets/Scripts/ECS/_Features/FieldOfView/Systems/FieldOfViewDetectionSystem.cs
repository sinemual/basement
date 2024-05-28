using Client.Data.Core;
using Leopotam.Ecs;

namespace Client
{
    public class FieldOfViewDetectionSystem : IEcsRunSystem
    {
        private SharedData _data;

        private EcsFilter<FieldOfViewProvider>.Exclude<DetectionState> _filter;
        private EcsFilter<FieldOfViewProvider, DetectionState> _checkFilter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var fieldOfViewProvider = ref entity.Get<FieldOfViewProvider>();

                if (fieldOfViewProvider.VisibleTargets.Count > 0)
                {
                    entity.Get<DetectionState>();
                    entity.Get<Timer<TimerToDetectionPlayer>>().Value = _data.BalanceData.DetectionTimer;
                }
            }

            foreach (var idx in _checkFilter)
            {
                ref var entity = ref _checkFilter.GetEntity(idx);
                ref var fieldOfViewProvider = ref entity.Get<FieldOfViewProvider>();

                if (fieldOfViewProvider.VisibleTargets.Count == 0)
                    if (entity.Has<DetectionState>() && entity.Has<Timer<TimerToDetectionPlayer>>())
                    {
                        entity.Del<DetectionState>();
                        entity.Del<Timer<TimerToDetectionPlayer>>();
                    }
            }
        }
    }
}