using Client.Data.Core;
using Client.ECS.CurrentGame.Mining;
using Leopotam.Ecs;
using UnityEditor.UIElements;

namespace Client
{
    public class TryDespawnBusyWorldUiAtTimerSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;

        private PrefabFactory _prefabFactory;

        private EcsFilter<TryDespawnBusyWorldUiAtTimerRequest, TimerDoneEvent<DespawnTimer>> _timerFilter;

        public void Run()
        {
            foreach (var idx in _timerFilter)
            {
                ref var entity = ref _timerFilter.GetEntity(idx);

                if (entity.Has<IsBusyMarker>())
                {
                    entity.Get<Timer<DespawnTimer>>().Value = _data.BalanceData.HideWorldUiTime;
                    entity.Del<IsBusyMarker>();
                }
                else
                {
                    entity.Get<TryDespawnBusyWorldUiAtTimerRequest>().DependedEntity.Del<TapProgressBar>();
                    entity.Del<TryDespawnBusyWorldUiAtTimerRequest>();
                    _prefabFactory.Despawn(ref entity);
                }
            }
        }
    }
}