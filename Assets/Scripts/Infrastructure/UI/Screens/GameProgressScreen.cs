using System;
using System.Collections.Generic;
using Client;
using Client.Data;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameProgressScreen : BaseScreen
{
    [SerializeField] private ActionButton hideScreenButton;
    [SerializeField] private List<Sprite> taskProgressSprites;
    [SerializeField] private List<GameProgressStepView> gameProgressStepViews;

    protected override void ManualStart()
    {
        hideScreenButton.OnClickEvent.AddListener(() =>
        {
            SetShowState(false);
        });
        
        OnShowScreen.AddListener(UpdateScreen);
    }

    public void UpdateScreen()
    {
        int counter = 0;
        foreach (var goal in SharedData.StaticData.GameProgressGoals)
        {
            gameProgressStepViews[counter].rewardImage.sprite = goal.RewardSprite;
            gameProgressStepViews[counter].descriptionText.text = goal.GoalDescriptionText;
            
            var tempCounterForButton = counter;
            gameProgressStepViews[counter].getRewardButton.OnClickEvent.RemoveAllListeners();
            gameProgressStepViews[counter].getRewardButton.OnClickEvent.AddListener(() => TakeProgressReward(tempCounterForButton));

            gameProgressStepViews[counter].backgroundImage.sprite = taskProgressSprites[0];
            if (SharedData.PlayerData.GameProgressData[counter].IsCompleted)
                gameProgressStepViews[counter].backgroundImage.sprite = taskProgressSprites[1];
            if (SharedData.PlayerData.GameProgressData[counter].IsRewardTaken)
                gameProgressStepViews[counter].backgroundImage.sprite = taskProgressSprites[2];
            
            counter++;
        }
        
        counter = 0;
        foreach (var gameProgressStepView in gameProgressStepViews)
        {
            CheckGoalStatus(SharedData.PlayerData.GameProgressData[counter], gameProgressStepView);
            counter++;
        }

    }

    public Transform GetSpawnProgressRewardPoint() => gameProgressStepViews[SharedData.PlayerData.GameProgressStep].getRewardButton.gameObject.transform;

    private void CheckGoalStatus(PlayerData.GoalStatusData data, GameProgressStepView view)
    {
        view.getRewardButton.gameObject.SetActive(false);

        if (data.IsCompleted && !data.IsRewardTaken)
        {
            view.getRewardButton.gameObject.SetActive(true);
        }
    }
    
    private void TakeProgressReward(int id)
    {
        GameUi.EventBus.GameProgressScreen.OnTakeProgressRewardButtonTap(SharedData.StaticData.GameProgressGoals[id]);
        UpdateScreen();
    }
}