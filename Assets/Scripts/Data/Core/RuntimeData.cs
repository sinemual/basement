using System;
using System.Linq;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.PlayerEquipment.Components;
using Data.Base;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Rendering;

namespace Data
{
    [Serializable]
    public class RuntimeData : BaseData
    {
        public GameState CurrentGameState;
        public int LastLevelIndex;

        public bool IsPlayerControl = true;
        public int CameraSide = 0;
        public bool IsBlockingRaycastForTutorial = false;
        public GoalData CurrentGoal;
        
        [Header("Current Level")]
        public int LevelGoalCounter;
        public int NeededLevelExperience;
        public int AllLevelExperience;
        public float CurrentLevelExperience;
        public bool IsCurrentLevelCompleted;
        public LocationType CurrentLocationType;
        public int CurrentMineLevelIndex;
        public int CurrentGlobalMapPositionIndex;
        public PlayerEquipType currentPlayerEquipMode;
        public PlayerData.ResourceAmountByType MinedLevelResources;
        
        [Header("Boosters")] 
        public bool IsTntPickaxeBoosterWork;
        public void InjectData(SharedData sharedData)
        {
            base.SharedData = sharedData;
        }

        public override void ResetData()
        {
        }

        public double GetLevelReward()
        {
            double value = SharedData.PlayerData.CurrentWarStepIndex * SharedData.BalanceData.BaseLevelReward;
            if (value < 100)
                value = 100;
            return value;
        }

        public void ResetLevelData()
        {
            CurrentLevelExperience = 0;
            NeededLevelExperience = 0;
            CameraSide = 0;
            IsCurrentLevelCompleted = false;
            
            MinedLevelResources = new PlayerData.ResourceAmountByType();
            foreach (ResourceType res in (ResourceType[])Enum.GetValues(typeof(ResourceType)))
                MinedLevelResources.Add(res, 0);

            IsTntPickaxeBoosterWork = false;
        }

        public float CalculateStat(ref EcsEntity entity, StatType statType)
        {
            float value = 0.0f;
            ref var baseStats = ref entity.Get<BaseStats>().Value;

            if (baseStats.ContainsKey(statType))
                value = baseStats[statType];

            if (entity.Has<Equipment>())
                foreach (var item in entity.Get<Equipment>().Value)
                    if (item.Value.Stats.TryGetValue(statType, out float statValue))
                        value += statValue;

            return value;
        }

        public bool IsPlayerHasAllResourcesForCraft(CraftRecipeData craftRecipeData)
        {
            var isIt = true;
            foreach (var neededItem in craftRecipeData.NeededItems)
            {
                if (neededItem.ItemData is ResourceItemData res)
                    if (SharedData.PlayerData.Resources[res.Type] < neededItem.Amount)
                        isIt = false;
            }

            return isIt;
        }

        public bool IsPlayerHasAllResourcesForBuild(BuildingData buildingData)
        {
            var isIt = true;
            foreach (var neededItem in buildingData.NeededItems)
            {
                if (neededItem.ItemData is ResourceItemData res)
                    if (SharedData.PlayerData.Resources[res.Type] < neededItem.Amount)
                        isIt = false;
            }

            return isIt;
        }


        /*public int ExperienceToNextLevel() => 
            ExperienceToLevel(SharedData.PlayerData.PlayerLevel + 1) - ExperienceToLevel(SharedData.PlayerData.PlayerLevel);

        public int ExperienceToLevel(int level) => 
            (int) (SharedData.BalanceData.LevelBaseCoef * Mathf.Pow(SharedData.BalanceData.LevelMultiCoef, level));*/

        public bool IsPlayerHasAllResourcesForAnyBuild()
        {
            var isIt = false;
            foreach (BuildingType buildingType in (BuildingType[])Enum.GetValues(typeof(BuildingType)))
            foreach (var build in SharedData.StaticData.BuildingsData[buildingType].Value)
            {
                var isItForBuild = true;

                if (SharedData.PlayerData.BuildingsSaveData[build.Type].Status == BuildingStatus.Builded)
                    continue;

                foreach (var neededItem in build.NeededItems)
                {
                    if (neededItem.ItemData is ResourceItemData res)
                        if (SharedData.PlayerData.Resources[res.Type] < neededItem.Amount)
                            isItForBuild = false;
                }

                if (isItForBuild)
                    return true;
            }

            return isIt;
        }

        /*public bool IsPlayerHasNewOpenLocation()
        {
            return (SharedData.PlayerData.PlayerLevel >=
                    SharedData.StaticData.LocationNeededLevelData[(LocationType)((int)SharedData.RuntimeData.CurrentLocationType + 1)]);
        }*/
        
        
    }
}