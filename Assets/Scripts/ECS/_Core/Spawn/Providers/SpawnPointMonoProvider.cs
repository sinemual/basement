using UnityEngine;

public class SpawnPointMonoProvider : MonoProvider<SpawnPointProvider>
{
#if UNITY_EDITOR
    public void OnValidate()
    {
        if (Value.Value == null)
            Value.Value = GetComponent<Transform>();
    }
#endif
}