using System;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

public class TMonoProvider : MonoProvider<TProvider>
{
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Value.RootBone != null)
            if (Value.BodyParts.Count == 0)
            {
                Value.BodyParts = new List<Rigidbody>();
                Value.BodyParts.AddRange(Value.RootBone.GetComponentsInChildren<Rigidbody>());
            }
    }
#endif
}

[Serializable]
public struct TProvider
{
    public Transform RootBone;
    public List<Rigidbody> BodyParts;
}
