using System;

public class GlobalMapScreenEvents
{
    public event Action StartNextLevelButton;
    public event Action GoToVillageButton;
    public void OnStartNextLevelButton() => StartNextLevelButton?.Invoke();
    public void OnGoToVillageButton() => GoToVillageButton?.Invoke();
}