using Client.Data;
using Client.Data.Core;
using Leopotam.Ecs;

namespace Client
{
    public class InitGameSystem : IEcsInitSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;
        private CameraService _cameraService;
        private UserInterfaceEventBus _uiEventBus;
        
        public void Init()
        {
            _data.RuntimeData.ResetLevelData();
            
            _data.RuntimeData.CurrentGameState = GameState.OnLevel;
            _ui.ChooseItemScreen.SetShowState(true);
            //game state
            /*if (_data.PlayerData.IsMechanicsTutorialComplete)
            {
                
            }*/
            /*else if (_data.PlayerData.IsMechanicsTutorialComplete && !_data.PlayerData.IsMetaTutorialComplete)
                _data.RuntimeData.CurrentGameState = GameState.Village;
            else if (!_data.PlayerData.IsMechanicsTutorialComplete && !_data.PlayerData.IsMetaTutorialComplete)
            {
                _data.PlayerData.OpenLocations[LocationType.Tutorial] = true;
                _data.RuntimeData.CurrentLocationType = LocationType.Tutorial;
                _world.NewEntity().Get<SpawnLevelRequest>();
            }*/

            _world.NewEntity().Get<CheckGoalStateEvent>();
            _world.NewEntity().Get<SpawnLevelRequest>();
            //ui
            _ui.OpenSettingScreen.SetShowState(true);
               // _ui.OpenInventoryScreen.SetShowState(true);
            _ui.OpenCraftScreen.SetShowState(true);
            _ui.OnLevelScreen.SetShowState(true);
            _uiEventBus.Resources.OnChangeResourceAmount();
            
            //_ui.GoalScreen.SetShowState(true);
            //_ui.GoalScreen.UpdateScreen();
        }
    }
}