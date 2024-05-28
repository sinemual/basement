using System;
using UnityEngine;

namespace Client.Data.Equip
{
    [CreateAssetMenu(menuName = "MineTap/Usable/TNT", fileName = "item_tnt")]
    [Serializable]
    public class TntItemData : ItemData, IUsable
    {
        public float ExplosionRadius;
        public StatValue Stats;
    }
}