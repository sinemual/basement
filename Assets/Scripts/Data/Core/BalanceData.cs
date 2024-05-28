using System;
using System.Collections.Generic;
using Client;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Data.Base;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/BalanceData", fileName = "BalanceData")]
    public class BalanceData : BaseDataSO
    {
        [Header("Characters")] 
        public float ArrowSpeed;
        public float ArrowShotChance;
        
        public float PathMovementRotateSpeed;
        public float DurabilityLossCoef;

        [Header("Time")] 
        public float DetectionTimer;
        public float TimeToDespawnDeadBody;
        public float DespawnVfxTime;
        public float DespawnMiningViewTime;
        public float HideWorldUiTime;
        public float TapReloadTime;

        [Header("Numbers")] 
        public double StartMoney;
        public double BaseLevelReward;
        public float LevelMultiCoef;
        public float LevelBaseCoef;
        public float LevelDoneExperienceCoef;
        
        [Header("Chest")] 
        public float StartFlyRandomTime;
        public float EndFlyRandomTime;
        public float FlyRandomChance;
        
        [Header("TimersTime")] 
        public TimeSpan ResetInterDailyCapTime = new TimeSpan(24, 0, 0);

        [Header("Physic Balance")]
        public float PushForceCoef;
        public float DeadForceCoef;
        public float PushForceUpCoef;
        
        [Header("Throw Trajectory")]
        public int  LinePoints;
        public float TimeBetweenPoints;
        public float ThrowStrength;
        public float ThrowMass;
        public float AimSpeed;

        [Header("Notifications")]
        [Tooltip("Push notification time (format: hh:mm) example - 20:00)")]
        public string PushNotificationTime;
        
        public float DropItemSpeed;

        public void InjectData(SharedData sharedData)
        {
        }
    }
}