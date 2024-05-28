using System;
using Client.Data;
using Data.Base;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/InterstitialSettingsData", fileName = "InterstitialSettingsData")]
    [Serializable]
    public class InterstitialSettingsData : BaseDataSO
    {
        public int StartLevel;
        public float Interval;
        public float DailyCap;
        
        public bool IsShowForNextLevelTap;
        public bool IsShowForGoToVillageTap;
        public bool IsShowForGoToGlobalMapTap;
        public bool IsShowForExitLevelTap;
    }
}