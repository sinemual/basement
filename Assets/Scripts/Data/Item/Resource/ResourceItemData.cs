using System;
using UnityEngine;

namespace Client.Data.Equip
{
    [CreateAssetMenu(menuName = "MineTap/ResourceItemData", fileName = "ResourceItemData")]
    [Serializable]
    public class ResourceItemData : ItemData
    {
        public ResourceType Type;
    }
}