using Leopotam.Ecs;

public class GameObjectMonoProvider : MonoProvider<GameObjectProvider>
{
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Value.Value == null)
            Value = new GameObjectProvider
            {
                Value = gameObject
            };
    }
#endif

    public override void Provide(ref EcsEntity entity)
    {
        entity.Get<GameObjectProvider>() = new GameObjectProvider
        {
            Value = gameObject
        };
    }
}