using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client.Data.Equip
{
    [CreateAssetMenu(menuName = "MineTap/CraftRecipeData", fileName = "CraftRecipeData")]
    [Serializable]
    public class CraftRecipeData : ItemData
    {
        public ItemData GettedItem;
        public List<ItemWithAmount> NeededItems;

        private void OnValidate()
        {
            Id = name.ToLower().Replace(' ', '-');
        }
    }
}