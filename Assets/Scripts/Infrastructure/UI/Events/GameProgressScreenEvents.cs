using System;
using Data;

public class GameProgressScreenEvents
{
    public event Action<GoalData> TakeProgressRewardButtonTap;
    public void OnTakeProgressRewardButtonTap(GoalData data) => TakeProgressRewardButtonTap?.Invoke(data);
}