using System.Collections.Generic;
using Client.Data.Equip;
using Data.Base;
using UnityEngine;

namespace Client.Data
{
    [CreateAssetMenu(menuName = "MineTap/BlockData", fileName = "BlockData")]
    public class BlockData : ScriptableObject
    {
        public BlockType Type;
        public int Level;
        public StatValue Stats;
        public ItemView View;
        public float ExpMultiplierForLevelComplete;
        public List<Drop> Loot;
    }
}