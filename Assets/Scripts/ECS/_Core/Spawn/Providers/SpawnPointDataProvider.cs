using System;
using Client.Data;
using UnityEngine;

[Serializable]
public struct SpawnPointDataProvider
{
    public GameObject Prefab;
    public int Id;
    public float Chance;
}