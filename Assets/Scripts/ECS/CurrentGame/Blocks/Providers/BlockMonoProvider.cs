using System.Collections.Generic;
using Client.Data;
using UnityEngine;

public class BlockMonoProvider : MonoProvider<BlockProvider>
{
    private void OnValidate()
    {
        if (Value.Model == null)
            Value.Model = gameObject;
    }
}