using System;
using System.Collections.Generic;
using Data.Base;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Client.Data.Equip
{
    [CreateAssetMenu(menuName = "MineTap/BuildingData", fileName = "BuildingData")]
    [Serializable]
    public class BuildingData : BaseDataSO
    {
        public string Id;
        public int Level;
        public BuildingType Type;
        public string Name;
        public string Description;
        public float BuildTimeInSec;
        public float ProduceTimeInSec;
        public Sprite ViewSprite;
        public List<CraftRecipeData> GettedCraftRecipes;

        public List<ItemWithAmount> NeededItems;
        public ItemWithAmount ProductionItem;

#if UNITY_EDITOR
        private void OnValidate()
        {
            Id = name.ToLower().Replace(' ', '-');
            //SetDirty();
        }
#endif
    }
}