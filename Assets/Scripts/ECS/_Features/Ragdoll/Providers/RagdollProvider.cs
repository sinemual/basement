using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RagdollProvider
{
    public Animator Animator;
    public Collider MainCollider;
    public Rigidbody MainRigidbody;
    public Transform RootBone;
    public List<Rigidbody> BodyParts;
}