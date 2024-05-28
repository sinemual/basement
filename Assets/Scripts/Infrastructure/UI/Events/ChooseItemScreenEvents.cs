using System;
using Client.Data.Equip;

public class ChooseItemScreenEvents
{
    public event Action<ItemData> ChooseItemButtonTap;
    public void OnChooseItemButtonTap(ItemData itemData) => ChooseItemButtonTap?.Invoke(itemData);
}