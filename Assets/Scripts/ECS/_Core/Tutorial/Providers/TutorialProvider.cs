using System;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

[Serializable]
public struct TutorialProvider
{
    public TutorialStep TutorialStep;
    public List<GameObject> TutorialGameObjects;
    public List<GameObject> TutorialLayerGameObjects;
}