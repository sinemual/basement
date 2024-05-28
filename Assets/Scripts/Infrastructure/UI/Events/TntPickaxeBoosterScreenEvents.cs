using System;

public class TntPickaxeBoosterScreenEvents
{
    public event Action ActivateTntPickaxeBoosterButtonTap;
    public void OnActivateTntPickaxeBoosterButtonTap() => ActivateTntPickaxeBoosterButtonTap?.Invoke();
}