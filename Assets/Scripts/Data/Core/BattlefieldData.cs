using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Data
{
    [CreateAssetMenu(menuName = "GameData/BattlefieldData", fileName = "BattlefieldData")]
    public class BattlefieldData : ScriptableObject
    {
        public Vector2 LevelRange;
        public Vector2 WeaponLevelRange;
        public Vector2 ArmorLevelRange;
        public float Difficult;
        public BattlefieldSoldierTypeAmountData SoldiersAmount;
    }
    
    [Serializable]
    public class BattlefieldSoldierTypeAmountData : SerializedDictionary<SoldierType, int>
    {
    }
}