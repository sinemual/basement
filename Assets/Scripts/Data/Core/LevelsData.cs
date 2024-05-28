using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Data
{
    [CreateAssetMenu(menuName = "GameData/LevelsData", fileName = "LevelsData")]
    public class LevelsData : ScriptableObject
    {
        public List<MonoEntity>  Levels;
        public LevelsByLocationType  RandomLevels;
        public int AlwaysLoadLevelId;
    }

    [Serializable]
    public class LevelsByLocationType : SerializedDictionary<LocationType, LevelList>
    {
    }

    [Serializable]
    public class LevelList
    {
        public List<MonoEntity> Value;
    }
}