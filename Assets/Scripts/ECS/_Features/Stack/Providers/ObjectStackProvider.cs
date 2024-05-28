using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

[Serializable]
public struct ObjectStackProvider
{
    public int Rows;
    public int Columns;
    public int ObjectsInColumn;
    public Vector3 ObjectsOffset;
    public List<Transform> Grid;
    [HideInInspector] public int Capacity;

    public List<EcsEntity> Objects;
}