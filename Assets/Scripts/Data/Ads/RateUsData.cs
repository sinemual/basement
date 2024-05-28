using System;
using System.Collections.Generic;
using Data.Base;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/RateUsData", fileName = "RateUsData")]
    [Serializable]
    public class RateUsData : BaseDataSO
    {
        public List<int> NumberOfLevelToShowPopup;
    }
}