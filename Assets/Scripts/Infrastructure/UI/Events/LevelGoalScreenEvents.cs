using System;

public class LevelGoalScreenEvents
{
    public event Action GoToTheNextLevelButtonTap;
    public void OnGoToTheNextLevelButtonTap() => GoToTheNextLevelButtonTap?.Invoke();
}