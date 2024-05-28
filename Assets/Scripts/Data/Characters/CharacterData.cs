using System.Collections.Generic;
using Client.Data.Equip;
using UnityEngine;

namespace Client.Data
{
    [CreateAssetMenu(menuName = "MineTap/CharacterData", fileName = "CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public CharacterType Type;
        public StatValue StartStats;
        public List<Drop> Loot;
    }
}