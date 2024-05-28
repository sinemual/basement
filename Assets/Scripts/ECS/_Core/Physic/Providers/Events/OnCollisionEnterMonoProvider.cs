using Leopotam.Ecs;
using UnityEngine;

public class OnCollisionEnterMonoProvider : MonoPhysicsProviderBase
{
    public void OnCollisionEnter(Collision other)
    {
        if (_entity.IsNull()) return;

        _entity.Get<OnCollisionEnterEvent>() = new OnCollisionEnterEvent
        {
            Collision = other,
            Sender = gameObject,
            CollisionName = gameObject.name
        };
    }
}