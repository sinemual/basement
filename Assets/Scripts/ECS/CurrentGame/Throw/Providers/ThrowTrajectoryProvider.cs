using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ThrowTrajectoryProvider
{
    public LineRenderer LineRenderer;
    public List<GameObject> LineObjects;
    public Transform TargetObject;
    public Transform StartPoint;
}