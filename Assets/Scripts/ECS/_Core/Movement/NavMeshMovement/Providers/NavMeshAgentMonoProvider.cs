using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentMonoProvider : MonoProvider<NavMeshAgentProvider>
{
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Value.Value == null)
            Value = new NavMeshAgentProvider
            {
                Value = GetComponent<NavMeshAgent>()
            };
    }
#endif
}