using System;

public class GameScreenEvents
{
    public event Action OpenCraftScreenButtonTap;
    public void OnOpenCraftScreenButtonTap() => OpenCraftScreenButtonTap?.Invoke();
}