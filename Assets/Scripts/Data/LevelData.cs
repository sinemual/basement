using System;
using Data.Base;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/LevelData", fileName = "LevelData")]
    [Serializable]
    public class LevelData : BaseDataSO
    {
        public int Id;

    }
}