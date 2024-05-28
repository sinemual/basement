using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMonoProvider : MonoProvider<RigidbodyProvider>
{
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Value.Value == null)
            Value = new RigidbodyProvider
            {
                Value = GetComponent<Rigidbody>()
            };
    }
#endif
}