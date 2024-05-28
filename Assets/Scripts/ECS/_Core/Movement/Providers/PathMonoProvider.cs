using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class PathMonoProvider : MonoProvider<PathProvider>
{
#if UNITY_EDITOR
    [Button]
    public void GetPath()
    {
        Value.Value.Clear();
        Value.Value.AddRange(GetComponentsInChildren<Transform>());
        Value.Value.RemoveAt(0);
    }

    /*private void OnValidate()
    {
        Value.Value.Clear();
        Value.Value.AddRange(GetComponentsInChildren<Transform>());
        Value.Value.RemoveAt(0);
    }*/
#endif
}