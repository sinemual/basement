using System;

public class LevelCompleteScreenEvents
{
    public event Action GetRewardAndGoToNextLevelButtonTap;
    public event Action StartNextLevelButton;
    public event Action HideScreenButton;
    public event Action BackToMetaButton;
    public void OnGetRewardAndGoToNextLevelButtonTap() => GetRewardAndGoToNextLevelButtonTap?.Invoke();
    public void OnStartNextLevelButton() => StartNextLevelButton?.Invoke();
    public void OnHideScreenButton() => HideScreenButton?.Invoke();
    public void OnBackToMetaButton() => BackToMetaButton?.Invoke();
}