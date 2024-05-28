using System.Collections.Generic;
using UnityEngine;

public class RagdollMonoProvider : MonoProvider<RagdollProvider>
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