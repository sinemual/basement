using Leopotam.Ecs;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Leopotam.Ecs.UnityIntegration;
using UnityEditor;
#endif
using UnityEngine;

public class MonoEntity : MonoProviderBase
{
    private MonoProviderBase[] _monoProviders;
    public EcsEntity Entity;

    public MonoProvider<T> Get<T>() where T : struct
    {
        foreach (MonoProviderBase link in _monoProviders)
            if (link is MonoProvider<T> monoLink)
                return monoLink;

        return null;
    }

    public override void Provide(ref EcsEntity entity)
    {
        this.Entity = entity;
        _monoProviders = GetComponents<MonoProviderBase>();
        foreach (MonoProviderBase monoProvider in _monoProviders)
        {
            if (monoProvider is MonoEntity) continue;
            monoProvider.Provide(ref entity);
        }
    }

#if UNITY_EDITOR
    [Button]
    private void SelectEntity()
    {
        GameObject selectedEntity = FindObjectOfType<EcsWorldObserver>().EntityGameObjects[Entity.GetInternalId()];
        if (selectedEntity != null)
            Selection.activeGameObject = selectedEntity;
        else
            Debug.LogWarning($"Entity of {gameObject.name} is null!");
    }
#endif
}