using Leopotam.Ecs;

public abstract class MonoPhysicsProviderBase : MonoProviderBase
{
    protected EcsEntity _entity;

    public override void Provide(ref EcsEntity entity)
    {
        _entity = entity;
    }
}