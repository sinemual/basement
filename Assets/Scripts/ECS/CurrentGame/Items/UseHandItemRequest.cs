using Client.Data.Equip;
using Leopotam.Ecs;

public struct UseHandItemRequest
{
    public EcsEntity ItemEntity;
    public ItemData Data;
}