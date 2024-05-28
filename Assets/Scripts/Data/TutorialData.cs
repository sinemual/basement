using System;
using Client.Data;
using Data.Base;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "MineTap/TutorialData", fileName = "TutorialData")]
    [Serializable]
    public class TutorialData : BaseDataSO
    {
        public bool Is3DTutorial;
        public bool IsBlockingAllRaycastExpectTutorial;
        public bool IsNextStepDependiced;

    }
}