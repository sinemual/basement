using Leopotam.Ecs;

namespace Client
{
    public class GameOverSystem : IEcsRunSystem
    {
        //private GameUI _ui;
        
        private EcsFilter<GameFailedEvent> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                var entity = _filter.GetEntity(idx);
                //_ui.SetShowStateLevelFailedScreen(true);
                entity.Del<GameFailedEvent>();
            }
        }
    }
}