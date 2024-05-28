using System;
using Client.Data.Equip;

public class GetItemScreenEvents
{
    public event Action<ItemData> GetItemButtonTap;
    public void OnGetItemButtonTap(ItemData itemData) => GetItemButtonTap?.Invoke(itemData);
}