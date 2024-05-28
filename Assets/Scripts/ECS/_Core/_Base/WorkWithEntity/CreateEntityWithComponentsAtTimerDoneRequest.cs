using Leopotam.Ecs;

namespace Client
{
    internal struct CreateEntityWithComponentsAtTimerDoneRequest<T> where T : struct
    {
        public EcsEntity BufferEntity;
        public bool IsHaveTargetEntity;
        public EcsEntity TargetEntity;
    }
}