using System;
using System.Linq;
using Client.Client;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Experience;
using Client.ECS.CurrentGame.Hit.Systems;
using Client.ECS.CurrentGame.Loot.Systems;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class StartTutorialSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;
        private GameUI _ui;

        private EcsFilter<GameStateChangedEvent> _filter;
        private EcsFilter<LevelCompleteEvent> _levelCompleteFilter;

        private EcsFilter<PlayerProvider> _playerFilter;
        private EcsFilter<TutorialProvider> _tutorialFilter;

        public void Run()
        {
            /*if (!_firstTimeScrollGlobalMapDoneEventFilter.IsEmpty())
            {
                _world.NewEntity().Get<StartTutorialRequest>().TutorialStep = TutorialStep.StartNextLocation;
                _ui.GlobalMapScreen.SetShowState(true);
            }*/
            
            if (_filter.IsEmpty())
                return;

            if (_data.RuntimeData.CurrentGameState == GameState.Village && !_data.PlayerData.TutrorialStates[TutorialStep.OpenBuildScreen])
            {
                _world.NewEntity().Get<AddResourceRequest>() = new AddResourceRequest()
                {
                    Type = ResourceType.Wood,
                    Amount = 20
                };
                _world.NewEntity().Get<AddResourceRequest>() = new AddResourceRequest()
                {
                    Type = ResourceType.Stone,
                    Amount = 10
                };
                _ui.VillageScreen.GoToGlobalScreenButton.SetShowState(false);
                _world.NewEntity().Get<StartTutorialRequest>().TutorialStep = TutorialStep.OpenBuildScreen;
            }

            if (_data.RuntimeData.CurrentGameState == GameState.OnLevel)
            {
                if (_data.PlayerData.EventLevelIndex == 0)
                    if (!_data.PlayerData.TutrorialStates[TutorialStep.Mining])
                    {
                        _world.NewEntity().Get<StartTutorialRequest>().TutorialStep = TutorialStep.Mining;
                        //_ui.CameraControlScreen.SetShowState(false);
                        _ui.OpenCraftScreen.SetShowState(false);
                        _ui.CraftScreen.SetShowState(false);
                       // _ui.OpenExitFromLevelScreen.SetShowState(false);
                    }
                    else
                    {
                        foreach (var tutr in _tutorialFilter)
                        foreach (var go in _tutorialFilter.Get1(tutr).TutorialLayerGameObjects)
                            go.gameObject.layer = _data.StaticData.GetRaycastLayer;
                    }
                
                if (!_levelCompleteFilter.IsEmpty() && _data.PlayerData.EventLevelIndex == 0)
                    if (!_data.PlayerData.TutrorialStates[TutorialStep.GoToTheNextLevel])
                    {
                        _world.NewEntity().Get<StartTutorialRequest>().TutorialStep = TutorialStep.GoToTheNextLevel;
                        //_ui.LevelCompleteScreen.SetShowState(false);
                    }

                if (_data.PlayerData.EventLevelIndex == 1)
                    if (!_data.PlayerData.TutrorialStates[TutorialStep.CraftPickaxe])
                    {
                        
                        _world.NewEntity().Get<AddResourceRequest>() = new AddResourceRequest()
                        {
                            Type = ResourceType.Wood,
                            Amount = 10
                        };
                        
                        _world.NewEntity().Get<StartTutorialRequest>().TutorialStep = TutorialStep.CraftPickaxe;
                        //_ui.CameraControlScreen.SetShowState(false);
                    }

                if (_data.PlayerData.EventLevelIndex == 2)
                    if (!_data.PlayerData.TutrorialStates[TutorialStep.Combat])
                    {
                        _world.NewEntity().Get<StartTutorialRequest>().TutorialStep = TutorialStep.Combat;
                        //_ui.CameraControlScreen.SetShowState(false);
                    }
                    else
                    {
                        foreach (var tutr in _tutorialFilter)
                        foreach (var go in _tutorialFilter.Get1(tutr).TutorialLayerGameObjects)
                            go.gameObject.layer = _data.StaticData.GetRaycastLayer;
                    }

                if (_data.PlayerData.EventLevelIndex == 3)
                    if (!_data.PlayerData.TutrorialStates[TutorialStep.CameraControl])
                    {
                        _world.NewEntity().Get<StartTutorialRequest>().TutorialStep = TutorialStep.CameraControl;
                    }

                if (_data.PlayerData.EventLevelIndex == 4)
                    if (!_data.PlayerData.TutrorialStates[TutorialStep.UseItems])
                    {
                        _playerFilter.GetEntity(0).Get<AddItemToInventoryRequest>().Value =
                            _data.StaticData.ItemDatabase.First(x => (x.Id == "item_tnt_0"));
                        _world.NewEntity().Get<StartTutorialRequest>().TutorialStep = TutorialStep.UseItems;
                    }
            }
        }

        private void TurnOffOpenScreenButtons()
        {
            
        }
    }
}