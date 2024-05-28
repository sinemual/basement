using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Data.Equip
{
    [CreateAssetMenu(menuName = "MineTap/ChestItemData", fileName = "ChestItemData")]
    [Serializable]
    public class ChestItemData : ItemData
    {
        public List<Drop> Loot;
    }
}