using System;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Mining;
using Leopotam.Ecs;

namespace Client
{
    public class CheckLevelGoalCompleteSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;
        
        private UserInterfaceEventBus _uiEventBus;
        
        private EcsFilter<MinedEvent> _mineFilter;
        private EcsFilter<DeadEvent> _killFilter;
        private EcsFilter<ItemCraftedEvent> _craftFilter;
        private EcsFilter<ItemUsedEvent> _useFilter;
        private EcsFilter<ChestFoundEvent> _chestFilter;

        public void Run()
        {
            if (_data.RuntimeData.CurrentGameState != GameState.OnLevel)
                return;
            
            foreach (var evnt in _mineFilter)
            {
                if (_data.RuntimeData.CurrentGoal.Type == GoalType.Mining)
                    if (_mineFilter.Get1(evnt).BlockType == _data.RuntimeData.CurrentGoal.BlockType)
                        AddProgressToGoalAndCheck();
            }

            foreach (var evnt in _killFilter)
            {
                if (_data.RuntimeData.CurrentGoal.Type == GoalType.Kill)
                    if (_killFilter.Get1(evnt).CharacterType == _data.RuntimeData.CurrentGoal.CharacterType)
                        AddProgressToGoalAndCheck();
            }

            foreach (var evnt in _craftFilter)
            {
                if (_data.RuntimeData.CurrentGoal.Type == GoalType.Craft)
                    if (_craftFilter.Get1(evnt).Value == _data.RuntimeData.CurrentGoal.CraftedItemData)
                        AddProgressToGoalAndCheck();
            }

            foreach (var evnt in _useFilter)
            {
                if (_data.RuntimeData.CurrentGoal.Type == GoalType.UseTnt)
                    if (_useFilter.Get1(evnt).Value is TntItemData)
                        AddProgressToGoalAndCheck();
            }

            foreach (var evnt in _chestFilter)
            {
                if (_data.RuntimeData.CurrentGoal.Type == GoalType.FindTheChest)
                {
                    _data.RuntimeData.CurrentGameState = GameState.LevelComplete;
                    AddProgressToGoalAndCheck();
                }
            }
        }

        private void AddProgressToGoalAndCheck()
        {
            _data.RuntimeData.LevelGoalCounter += 1;
            if (_data.RuntimeData.LevelGoalCounter >= _data.RuntimeData.CurrentGoal.GoalValue)
            {
                //_ui.OnLevelScreen.BackToVillageScreenButton.SetShowState(true);
                //_world.NewEntity().Get<LevelGoalCompleteEvent>().Type = _data.RuntimeData.CurrentGoal.Type;
            }
        }
    }
}