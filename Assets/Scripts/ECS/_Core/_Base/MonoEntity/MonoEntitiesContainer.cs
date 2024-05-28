using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

public class MonoEntitiesContainer : MonoBehaviour
{
    public List<MonoEntity> MonoEntities;
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        //if (MonoEntities == null)
        //{
            MonoEntities = new List<MonoEntity>();
            MonoEntities.AddRange(GetComponentsInChildren<MonoEntity>(true));
            MonoEntities.RemoveAt(0);
        //}
    }
#endif
    
    public void DisposeMonoEntityChildren()
    {
        foreach (var child in MonoEntities)
            child.Entity.Destroy();
    }
    
    public void ProvideMonoEntityChildren(EcsWorld world)
    {
        for (int i = 0; i < MonoEntities.Count; i++)
        {
            EcsEntity entity = world.NewEntity();
            MonoEntities[i].Provide(ref entity);
            entity.Get<FromThisLevelTag>();
        }
    }
}