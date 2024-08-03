using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Data.Equip
{
    [CreateAssetMenu(menuName = "MineTap/EquipItemData", fileName = "EquipItemData")]
    public class EquipItemData : ItemData
    {
        public PlayerEquipType Type;
        public StatValue Stats;
    }
}
