using Client.DevTools.MyTools;
using Leopotam.Ecs;
using UnityEngine;

public class CleanService
{
    private PoolService _poolService;

    public CleanService(PoolService poolService)
    {
        _poolService = poolService;
    }

    public void DespawnGameObject(GameObject go)
    {
        PrepareEcsEntity(go);
        _poolService.PutGameObjectToPool(go);
        go.SetActive(false);
    }

    public void DestroyGameObject(GameObject go)
    {
        if (go && go.TryGetComponent(out MonoEntity monoEntity))
            monoEntity.Entity.Destroy();

        Object.DestroyImmediate(go);
    }

    private void PrepareEcsEntity(GameObject go)
    {
        if (go && go.TryGetComponent(out MonoEntity monoEntity))
        {
            ref var entity = ref monoEntity.Entity;

            if (entity.Has<RigidbodyProvider>())
                Utility.ResetRigibodyVelocity(entity.Get<RigidbodyProvider>().Value);

            if (entity.Has<AnimatorProvider>())
                Utility.Animation.ResetAnimator(entity.Get<AnimatorProvider>().Value);

            if (go.TryGetComponent(out MonoEntitiesContainer monoEntitiesContainer))
                monoEntitiesContainer.DisposeMonoEntityChildren();

            entity.Destroy();
        }
    }
}