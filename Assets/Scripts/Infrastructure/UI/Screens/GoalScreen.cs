using System.Collections.Generic;
using Data;
using UnityEngine;

public class GoalScreen : BaseScreen
{
    [SerializeField] private Sprite backgroundRewardSprite;
    [SerializeField] private Sprite readyRewardSprite;
    [SerializeField] private GameProgressStepView gameProgressStepView;

    protected override void ManualStart()
    {
        OnShowScreen.AddListener(UpdateScreen);
    }

    public void UpdateScreen()
    {
        var goalNum = SharedData.PlayerData.GameProgressStep;
        var goalData = SharedData.StaticData.GameProgressGoals[goalNum];
        gameProgressStepView.rewardImage.sprite = goalData.RewardSprite;
        gameProgressStepView.backgroundImage.sprite = backgroundRewardSprite;
        gameProgressStepView.descriptionText.text = goalData.GoalDescriptionText;

        gameProgressStepView.getRewardButton.OnClickEvent.RemoveAllListeners();
        gameProgressStepView.getRewardButton.OnClickEvent.AddListener(() => TakeProgressReward(goalNum));
        CheckGoalStatus(SharedData.PlayerData.GameProgressData[goalNum], gameProgressStepView);
    }

    public Transform GetSpawnProgressRewardPoint() =>
        gameProgressStepView.getRewardButton.gameObject.transform;

    private void CheckGoalStatus(PlayerData.GoalStatusData data, GameProgressStepView view)
    {
        view.getRewardButton.gameObject.SetActive(false);

        if (data.IsCompleted && !data.IsRewardTaken)
        {
            view.getRewardButton.gameObject.SetActive(true);
            gameProgressStepView.backgroundImage.sprite = readyRewardSprite;
        }
    }

    private void TakeProgressReward(int id)
    {
        GameUi.EventBus.GameProgressScreen.OnTakeProgressRewardButtonTap(SharedData.StaticData.GameProgressGoals[id]);
    }
}