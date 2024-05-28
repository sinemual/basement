using Leopotam.Ecs;

public abstract class MonoProvider<T> : MonoProviderBase where T : struct
{
    public T Value;

    public override void Provide(ref EcsEntity entity)
    {
        entity.Get<T>() = Value;
    }
}