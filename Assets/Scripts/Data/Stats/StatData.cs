using Client.Data.Core;
using UnityEngine;

namespace Client.Data
{
    [CreateAssetMenu(menuName = "MineTap/StatData", fileName = "StatData")]
    public class StatData : ScriptableObject
    {
        public StatType Type;
        public Sprite Icon;
    }
}