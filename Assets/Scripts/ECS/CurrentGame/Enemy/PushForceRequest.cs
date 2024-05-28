using Leopotam.Ecs;

namespace Client.ECS.CurrentGame.Mining
{
    internal struct PushForceRequest
    {
        public EcsEntity Source;
        public float Force;
    }
}