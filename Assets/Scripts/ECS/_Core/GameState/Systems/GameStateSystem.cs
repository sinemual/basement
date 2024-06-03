using Client.Data;
using Client.Data.Core;
using Client.DevTools.MyTools;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;
using CameraType = Client.Data.CameraType;

namespace Client
{
    public class GameStateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;
        private GameUI _ui;

        private CameraService _cameraService;
        private AnalyticService _analyticService;
        private AudioService _audioService;

        private GameState _previousGameState;

        public void Init()
        {
            _previousGameState = GameState.Init;
        }

        public void Run()
        {
            if (_previousGameState != _data.RuntimeData.CurrentGameState)
            {
                _previousGameState = _data.RuntimeData.CurrentGameState;
                _world.NewEntity().Get<GameStateChangedEvent>().CurrentGameState = _data.RuntimeData.CurrentGameState;

                if (_data.RuntimeData.CurrentGameState == GameState.Village)
                {
                    _world.NewEntity().Get<DisposeLevelRequest>();
                    _data.SceneData.Village.SetActive(true);
                    _data.SceneData.GlobalMap.SetActive(false);
                    _ui.GlobalMapScreen.SetShowState(false);
                    _ui.VillageScreen.SetShowState(true);
                    _ui.GoalScreen.SetShowState(true);
                    _ui.OpenCraftScreen.SetShowState(false);
                    _ui.OpenTntPickaxeBoosterScreen.SetShowState(false);
                    _ui.BoosterFeelingScreen.SetShowState(false);
                    _cameraService.SetCamera(CameraType.VillageCamera, null, null);
                    _audioService.StopAllAmbient();
                    _analyticService.LogEvent("go_to_village");
                }

                if (_data.RuntimeData.CurrentGameState == GameState.GlobalMap)
                {
                    _world.NewEntity().Get<DisposeLevelRequest>();
                    _data.SceneData.Village.SetActive(false);
                    _data.SceneData.GlobalMap.SetActive(true);

                    _ui.GoalScreen.SetShowState(true);
                    _ui.GlobalMapScreen.SetShowState(true);
                    _ui.VillageScreen.SetShowState(false);
                    _ui.OpenTntPickaxeBoosterScreen.SetShowState(false);
                    _ui.CraftScreen.SetShowState(false);
                    _ui.OpenCraftScreen.SetShowState(false);
                    _ui.BoosterFeelingScreen.SetShowState(false);
                    _ui.GlobalMapScreen.GoToVillageButton.gameObject.SetActive(_data.PlayerData.TutrorialStates[TutorialStep.GoToMeta2]);
                    _cameraService.SetCamera(CameraType.GlobalMapCamera, null, null);
                    _audioService.StopAllAmbient();
                    _analyticService.LogEvent("go_to_global_map");
                }

                if (_data.RuntimeData.CurrentGameState == GameState.OnLevel)
                {
                    _data.SceneData.Village.SetActive(false);
                    _data.SceneData.GlobalMap.SetActive(false);

                    _ui.GoalScreen.SetShowState(false);
                    _ui.GlobalMapScreen.SetShowState(false);
                    _ui.VillageScreen.SetShowState(false);
                }
            }
        }
    }
}