using System;
using Client.Data.Equip;

public class ChangeItemScreenEvents
{
    public event Action ChooseItemButtonTap;
    public event Action<PlayerEquipType> ChangeItem;
    public void OnChangeItemButtonTap() => ChooseItemButtonTap?.Invoke();
    public void OnChangeItem(PlayerEquipType x) => ChangeItem?.Invoke(x);
}