using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PathProvider
{
    public List<Transform> Value;
    public bool IsLoop;
    public bool IsPoolingInTheEnd;
    public bool IsIdleAtPathEnd;
    public bool IsTeleportToBeginPath;
    public bool IsGoToBack;
    public bool IsNotFaceToDirection;
    public SpawnPointMonoProvider SpawnPointMonoProvider;
}