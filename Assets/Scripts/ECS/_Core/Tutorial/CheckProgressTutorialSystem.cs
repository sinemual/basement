using System.Linq;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Experience;
using Client.ECS.CurrentGame.Hit.Systems;
using Client.ECS.CurrentGame.Loot.Systems;
using Client.ECS.CurrentGame.Mining;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class CheckProgressTutorialSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;
        private GameUI _ui;

        private UserInterfaceEventBus _uiEventBus;

        private EcsFilter<PlayerProvider> _playerFilter;
        
        private EcsFilter<MinedEvent> _mineFilter;
        private EcsFilter<DeadEvent> _killFilter;
        private EcsFilter<ItemCraftedEvent> _craftFilter;
        private EcsFilter<ItemUsedEvent> _useFilter;
        private EcsFilter<RotateCameraEvent> _cameraFilter;
        private EcsFilter<PickBuildEvent> _pickBuildFilter;
        private EcsFilter<BuildEvent> _buildFilter;
        private EcsFilter<LevelCompleteEvent> _levelCompleteFilter;
        private EcsFilter<GameStateChangedEvent> _gameStateChangedFilter;
        private EcsFilter<TakeProgressRewardEvent> _takeProgressRewardFilter;

        public void Init()
        {
            /*_uiEventBus.GameScreen.OpenCraftScreenButtonTap += () =>
            {
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.OpenCraftScreen &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.OpenCraftScreen])
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;
            };*/

            _uiEventBus.LevelGoalScreen.GoToTheNextLevelButtonTap += () =>
            {
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.GoToMeta &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.GoToMeta])
                {
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;
                    _ui.ChestScreen.SetShowState(false);
                }
                
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.GoToMeta2 &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.GoToMeta2])
                {
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;
                    _ui.ChestScreen.SetShowState(false);
                }
            };
            
            _uiEventBus.LevelCompleteScreen.StartNextLevelButton += () =>
            {
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.GoToTheNextLevel &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.GoToTheNextLevel])
                {
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;
                    _ui.LevelCompleteScreen.SetShowState(false);
                }
            };
            
            _uiEventBus.GlobalMapScreen.StartNextLevelButton += () =>
            {
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.StartNextLocation &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.StartNextLocation])
                {
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;
                }
                
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.EndTutorialAndStartPlay &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.EndTutorialAndStartPlay])
                {
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;
                }
            };
            
            _uiEventBus.GlobalMapScreen.GoToVillageButton += () =>
            {
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.GoToVillage &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.GoToVillage])
                {
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;
                }
            };
            
            /*_uiEventBus.OpenGameProgressScreen.OpenGameProgressScreenButtonTap += () =>
            {
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.OpenGameProgressScreen &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.OpenGameProgressScreen])
                {
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;
                }
            };*/
            
            _uiEventBus.GameProgressScreen.TakeProgressRewardButtonTap += (_d) =>
            {
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.TakeGameProgressReward &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.TakeGameProgressReward])
                {
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;
                    _ui.Tutorials[TutorialStep.TakeGameProgressReward].SetShowState(false);
                    _ui.GameProgressScreen.SetShowState(false);
                }
            };
        }

        public void Run()
        {
            foreach (var idx in _mineFilter)
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.Mining &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.Mining])
                {
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;
                    _ui.OnLevelScreen.SetShowState(true);
                }
            
            foreach (var idx in _levelCompleteFilter)
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.CompleteLevel &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.CompleteLevel])
                {
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;
                }

            foreach (var idx in _killFilter)
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.Combat &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.Combat])
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;

            foreach (var idx in _craftFilter)
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.CraftPickaxe &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.CraftPickaxe])
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;

            foreach (var idx in _useFilter)
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.UseItems &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.UseItems])
                {
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;
                    _playerFilter.GetEntity(0).Get<AddItemToInventoryRequest>().Value =
                        _data.StaticData.ItemDatabase.First(x => (x.Id == "item_tnt_0"));
                }

            foreach (var idx in _cameraFilter)
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.CameraControl &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.CameraControl])
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;

            foreach (var idx in _pickBuildFilter)
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.OpenBuildScreen &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.OpenBuildScreen])
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;

            foreach (var idx in _buildFilter)
                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.Build &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.Build])
                {
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;
                    _ui.VillageScreen.GoToGlobalScreenButton.SetShowState(true);
                }

            foreach (var idx in _gameStateChangedFilter)
                if (_data.RuntimeData.CurrentGameState == GameState.GlobalMap &&
                    _data.PlayerData.CurrentTutorialStep == TutorialStep.GoToGlobalMap &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.GoToGlobalMap])
                {
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;
                    _ui.VillageScreen.SetShowState(false);
                }
            
            foreach (var idx in _gameStateChangedFilter)
                if (_data.RuntimeData.CurrentGameState == GameState.OnLevel &&
                    _data.PlayerData.CurrentTutorialStep == TutorialStep.EndTutorialAndStartPlay &&
                    !_data.PlayerData.TutrorialStates[TutorialStep.EndTutorialAndStartPlay])
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = _data.PlayerData.CurrentTutorialStep;
        }
    }
}