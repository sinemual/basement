using System;
using Data;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public struct LevelProvider
{
    public Transform CameraPointHandler;
    public Transform CameraPoint;
    public float CameraOrthoSize;
    public Vector3 CameraOffset;
    public LocationType LocationType;
}