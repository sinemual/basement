using System;

public class LevelFailedScreenEvents
{
    public event Action RestartLevelButtonTap;
    public event Action RewardReviveButtonTap;
    public void OnRestartLevelButtonTap() => RestartLevelButtonTap?.Invoke();
    public void OnRewardReviveButtonTap() => RewardReviveButtonTap?.Invoke();
}