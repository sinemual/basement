using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public struct CartoonSpringProvider
{
    [Header("Links")] public Transform SpringTransform;

    public Transform ObjectTransform;
    public Transform ObjectBody;

    [Header("Settings")] public bool IsRotate;

    public bool IsScale;
    [ShowIf("IsScale")] public Vector3 ScaleDown;
    [ShowIf("IsScale")] public Vector3 ScaleUp;
    [ShowIf("IsScale")] public float ScaleCoef;
    [ShowIf("IsRotate")] public float RotationCoef;
}