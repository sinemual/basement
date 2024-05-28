using System;

public class ExitFromLevelScreenEvents
{
    public event Action ExitFromLevelButtonTap;
    public void OnExitFromLevelButtonTap() => ExitFromLevelButtonTap?.Invoke();
}