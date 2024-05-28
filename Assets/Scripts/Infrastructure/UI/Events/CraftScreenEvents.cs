using System;
using Client.Data.Equip;

public class CraftScreenEvents
{
    public event Action<CraftRecipeData> CraftButtonTap;
    public event Action CloseCraftScreenButtonTap;
    public void OnCraftButtonTap(CraftRecipeData craftedItemData) => CraftButtonTap?.Invoke(craftedItemData);
    public void OnCloseCraftScreenButtonTap() => CloseCraftScreenButtonTap?.Invoke();
}