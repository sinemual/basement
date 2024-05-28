using System;
using UnityEngine;

[Serializable]
public struct ExplosionEnemyProvider
{
    public MonoEntity ExplosionSourceMonoEntity;
    public float Radius;
    public GameObject ExplosionGameObject;
}