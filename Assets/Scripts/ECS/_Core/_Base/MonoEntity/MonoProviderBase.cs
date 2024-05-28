using Leopotam.Ecs;
using UnityEngine;

public abstract class MonoProviderBase : MonoBehaviour
{
    public abstract void Provide(ref EcsEntity entity);
}