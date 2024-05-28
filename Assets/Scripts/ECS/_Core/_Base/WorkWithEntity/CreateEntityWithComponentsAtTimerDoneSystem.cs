using Client.Data.Core;
using Leopotam.Ecs;

namespace Client
{
    public class CreateEntityWithComponentsAtTimerDoneSystem<T> : IEcsRunSystem where T : struct
    {
        private EcsWorld _world;
        
        private EcsFilter<CreateEntityWithComponentsAtTimerDoneRequest<T>, TimerDoneEvent<T>> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var request = ref entity.Get<CreateEntityWithComponentsAtTimerDoneRequest<T>>();
                if (request.IsHaveTargetEntity)
                {
                    ref var targetEntity = ref request.TargetEntity;
                    request.BufferEntity.MoveTo(targetEntity);
                }
                else
                {
                    EcsEntity newEntity = _world.NewEntity();
                    request.BufferEntity.MoveTo(newEntity);
                }
            }
        }
    }
}