using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Data.Equip
{
    [CreateAssetMenu(menuName = "MineTap/EquipItemData", fileName = "EquipItemData")]
    public class EquipItemData : ItemData
    {
        public EquipType Type;
        public StatValue Stats;
    }
}
