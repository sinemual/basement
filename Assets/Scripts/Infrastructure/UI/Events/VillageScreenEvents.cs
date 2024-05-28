using System;

public class VillageScreenEvents
{
    public event Action GoToGlobalMapButtonTap;
    public void OnGoToGlobalMapButtonTap() => GoToGlobalMapButtonTap?.Invoke();

}