using System;
using Client.Data.Equip;

public class OpenChestScreenEvents
{
    public event Action<ItemData> OpenChestButtonTap;
    public void OnOpenChestButtonTap(ItemData itemData) => OpenChestButtonTap?.Invoke(itemData);
}