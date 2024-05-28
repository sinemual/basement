using System;
using System.Collections.Generic;
using Client.Data;
using Client.Data.Equip;
using Data.Base;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "MineTap/GoalData", fileName = "GoalData")]
    [Serializable]
    public class GoalData : BaseDataSO
    {
        public GoalType Type;
        public string GoalDescriptionText;
        public int GoalValue;
        
        public Sprite RewardSprite;
        public List<Drop> Reward;
        
        [ShowIf("@this.Type == GoalType.Mining")]
        public BlockType BlockType;
        
        [ShowIf("@this.Type == GoalType.Craft")]
        public ItemData CraftedItemData;
        
        [ShowIf("@this.Type == GoalType.Kill")]
        public CharacterType CharacterType;
        
        [ShowIf("@this.Type == GoalType.Build")]
        public BuildingType BuildingType;
    }
}