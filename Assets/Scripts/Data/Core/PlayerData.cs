using System;
using System.Collections.Generic;
using Client;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Data.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/PlayerData", fileName = "PlayerData")]
    public class PlayerData : SaveLoadDataSO, IResetable
    {
        [Header("GameSettings")]
        public bool IsGameLaunchedBefore;
        public bool IsVibrationOn;
        public bool IsSoundOn;
        public bool IsMechanicsTutorialComplete;
        public bool IsMetaTutorialComplete;
        public bool IsPlayerRatedGame;

        [Header("IAPs")] 
        public bool IsNoAdsIapBuyed;

        [Header("Numbers")] 
        public double Currency;
        public int CurrentLevelIndex;
        public bool IsRandomLevel;
        public int EventLevelIndex;
        public int InterstitalCounter;
        public int PlayerAvatarGlobalMapPositionIndex;
        public IsOpenLocations OpenLocations;

        [Header("Save Timers")] 
        public string OfflineTimeKey;
        public string DailyTimeKey;
        public string InterCapRestartTimeKey;
        public string FortuneWheelTimeKey;
        public string MonetizationPackTimeKey;
        public string DailyTasksTimeKey;
        
        [Header("Daily Tasks")] 
        public TutorialStep CurrentTutorialStep;
        public int GameProgressStep;

        [Header("Tutorials")] public TutorialStateData TutrorialStates;
        [Header("Tasks Data")] public List<GoalStatusData> GameProgressData;
        
        [Header("Equip")] public EquipLevelByType Equipment;
        [Header("Resources")] public ResourceAmountByType Resources;
        [Header("Craft")] public List<string> PlayerOpenedRecipes;
        public List<string> IsPlayerSeenRecipeItem;
        [Header("Inventory")] public InventoryData Inventory;
        [Header("Hand Item")] public string CurrentHandItemId;
        [Header("Building Save Data")] public BuildingSaveDataByType BuildingsSaveData;

        public void InjectData(SharedData sharedData)
        {
            base.SharedData = sharedData;

            InitDefaultValues();
        }

        private void InitDefaultValues()
        {
            Currency = SharedData.BalanceData.StartMoney;
            EventLevelIndex = 0;
            PlayerAvatarGlobalMapPositionIndex = 0;
            CurrentLevelIndex = 0;
            GameProgressStep = 0;
            IsRandomLevel = false;
            
            IsMechanicsTutorialComplete = false;
            IsMetaTutorialComplete = false;
            CurrentTutorialStep = TutorialStep.Mining;
            
            CurrentHandItemId = "";
            
            IsVibrationOn = true;
            IsSoundOn = true;   

            for (var i = 0; i < TutrorialStates.Count; i++)
                TutrorialStates[(TutorialStep)i] = false;
            
            PlayerOpenedRecipes = new List<string>();
            IsPlayerSeenRecipeItem = new List<string>();
            Inventory = new InventoryData();
            
            for (var i = 0; i < Resources.Count; i++)
                Resources[(ResourceType)i] = 0;

            for (var i = 0; i < Equipment.Count; i++)
                Equipment[(EquipType)i] = 0;
            
            for (var i = 0; i < OpenLocations.Count; i++)
                OpenLocations[(LocationType)i] = false;
            
            for (var i = 1; i < BuildingsSaveData.Count + 1; i++)
            {
                BuildingsSaveData[(BuildingType)i] = new BuildingSaveData
                {
                    Status = BuildingStatus.NotStarted,
                    CurrentLevel = -1,
                    IncomeTimes = 0,
                    BuildTimeKey = ""
                };
            }
            
            for (var i = 0; i < GameProgressData.Count; i++)
                GameProgressData[i] = new GoalStatusData
                {
                    IsCompleted = false,
                    IsRewardTaken = false,
                    CurrentValue = 0
                };
        }
        
        [Serializable]
        public class TutorialStateData : SerializedDictionary<TutorialStep, bool>
        {
        }
        
        [Serializable]
        public class IsOpenLocations : SerializedDictionary<LocationType, bool>
        {
        }
        
        [Serializable]
        public class EquipLevelByType : SerializedDictionary<EquipType, int>
        {
        }

        [Serializable]
        public class ResourceAmountByType : SerializedDictionary<ResourceType, int>
        {
        }
        
        [Serializable]
        public class BuildingSaveDataByType : SerializedDictionary<BuildingType, BuildingSaveData>
        {
        }

        [Serializable]
        public class InventoryData : SerializedDictionary<string, int>
        {
        }

        [Serializable]
        public class GoalStatusData
        {
            public bool IsCompleted;
            public bool IsRewardTaken;
            public int CurrentValue;
        }
        
        [Serializable]
        public class BuildingSaveData
        {
            public BuildingStatus Status;
            public string BuildTimeKey;
            public int CurrentLevel;
            public int IncomeTimes;
        }

        public void ResetData()
        {
            
        }
    }
}