using Leopotam.Ecs;

namespace Client
{
    public class TurnOffAtMovingCompleteSystem : IEcsRunSystem
    {
        private EcsFilter<TurnOffAtMovingCompleteRequest, MovingCompleteEvent> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref GameObjectProvider entityGo = ref entity.Get<GameObjectProvider>();
                entityGo.Value.SetActive(false);
            }
        }
    }
}