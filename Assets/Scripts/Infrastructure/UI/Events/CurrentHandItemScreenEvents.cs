using System;
using Client.Data.Equip;

public class CurrentHandItemScreenEvents
{
    public event Action UseItemButtonTap;
    public void OnUseItemButtonTap() => UseItemButtonTap?.Invoke();
}

public class InventoryScreenEvents
{
    public event Action<ItemData> ChooseItemButtonTap;
    public void OnChooseItemButtonTap(ItemData itemData) => ChooseItemButtonTap?.Invoke(itemData);
}