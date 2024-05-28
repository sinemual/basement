using System;
using Data.Base;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/RewardEquipUpgradeData", fileName = "RewardEquipUpgradeData")]
    [Serializable]
    public class RewardEquipUpgradeData : BaseDataSO
    {
        public float GeneralCooldown;
        public float ShowTimer;
        public AdsEquipUpgrade PickaxeData;
        public AdsEquipUpgrade SwordData;
        public AdsEquipUpgrade ShovelData;
        public AdsEquipUpgrade AxeData;

        [Serializable]
        public class AdsEquipUpgrade
        {
            public bool IsEnabled;
            public int Value;
            public int Cap;
            public float Cooldown;
        }
    }
}