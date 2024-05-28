using Client.Data;
using Client.Data.Core;
using Client.DevTools.MyTools;
using Leopotam.Ecs;

namespace Client
{
    public class LevelProgressSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;

        private EcsFilter<CheckLevelProgressRequest> _requestFilter;

        public void Run()
        {
            foreach (var request in _requestFilter)
            {
                /*bool levelClear = true;

                if (levelClear)
                    _world.NewEntity().Get<SetGameStateRequest>().NewGameState = StaticData.Enums.GameState.LevelComplete;

                _requestFilter.GetEntity(request).Del<CheckLevelProgressRequest>();*/
            }
        }
    }
}