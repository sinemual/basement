using Client;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;

public class LevelFailedScreen : BaseScreen
{
    [SerializeField] private ActionButton restartLevelButton;
    [SerializeField] private ActionButton rewardReviveButton;

    protected override void ManualStart()
    {
        restartLevelButton.OnClickEvent.AddListener(() =>
            {
                GameUi.EventBus.LevelFailedScreen.OnRestartLevelButtonTap();
                SetShowState(false);
            }
        );
        
        rewardReviveButton.OnClickEvent.AddListener(() =>
            {
                GameUi.EventBus.LevelFailedScreen.OnRewardReviveButtonTap();
                SetShowState(false);
            }
        );
        
        OnShowScreen.AddListener(UpdateScreen);
    }

    private void UpdateScreen()
    {
        rewardReviveButton.SetInteractable(GameUi.AdsService.IsRewardVideoReady());
    }
}