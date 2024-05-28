using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public struct FieldOfViewProvider
{
    public GameObject FieldOfViewGo;
    public float ViewRadius;
    [Range(0,360)]
    public float ViewAngle;
    
    public LayerMask TargetMask;
    public LayerMask ObstacleMask;
    
    [ReadOnly] public List<Transform> VisibleTargets;
    
    public float MeshResolution;
    public int EdgeResolveIterations;
    public float EdgeDstThreshold;

    public float MaskCutawayDst;

    public MeshFilter ViewMeshFilter;
    [HideInInspector] public Mesh ViewMesh;
}