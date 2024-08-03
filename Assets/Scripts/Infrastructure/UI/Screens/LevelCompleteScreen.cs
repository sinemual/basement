using System;
using System.Collections;
using Client;
using Client.Data;
using Client.Data.Equip;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using Client.DevTools.MyTools;

public class LevelCompleteScreen : BaseScreen
{
    [SerializeField] private bool isTutorialScreen;
    [SerializeField] private ActionButton getRewardAndGoToNextLevelButton;
    [SerializeField] private ActionButton hideScreenAndGoToNextLevelButton;
    [SerializeField] private ActionButton backToMetaButton;
    [SerializeField] private ActionButton hideScreenButton;
    [SerializeField] public GameObject newContentMark;
    [SerializeField] private TextMeshProUGUI levelCompleteText;
    [SerializeField] private ResourceScreen.ResourcePanelByType ResourcePanels;
    [SerializeField] private MinedResourcePanelByType MinedResourcePanels;

    private double LevelCompleteReward;

    protected override void ManualStart()
    {
        getRewardAndGoToNextLevelButton.OnClickEvent.AddListener(() =>
            {
                GameUi.EventBus.LevelCompleteScreen.OnGetRewardAndGoToNextLevelButtonTap();
                SetShowState(false);
                GameUi.OpenLevelCompleteScreen.SetShowState(false);
            }
        );

        hideScreenAndGoToNextLevelButton.OnClickEvent.AddListener(() =>
            {
                GameUi.EventBus.LevelCompleteScreen.OnStartNextLevelButton();
                SetShowState(false);
                GameUi.OpenLevelCompleteScreen.SetShowState(false);
            }
        );

        hideScreenButton.OnClickEvent.AddListener(() =>
            {
                GameUi.EventBus.LevelCompleteScreen.OnHideScreenButton();
                SetShowState(false);
                SharedData.RuntimeData.CurrentGameState = GameState.OnLevel;
                GameUi.OpenLevelCompleteScreen.SetShowState(true);
            }
        );

        backToMetaButton.OnClickEvent.AddListener(() =>
        {
            GameUi.EventBus.LevelCompleteScreen.OnBackToMetaButton();
            GameUi.EventBus.LevelGoalScreen.OnGoToTheNextLevelButtonTap();
            GameUi.OpenLevelCompleteScreen.SetShowState(false);
            SetShowState(false);
        });

        OnShowScreen.AddListener(UpdateScreen);
    }

    private void UpdateScreen()
    {
        newContentMark.gameObject.SetActive(false);
        //LevelCompleteReward = SharedData.RuntimeData.GetLevelReward();
        //rewardMoneyText.text = $"{Utility.FormatMoney(LevelCompleteReward)}";
        levelCompleteText.text = $"LEVEL {SharedData.PlayerData.CurrentWarStepIndex + 1}\nCOMPLETE";

        if (SharedData.RuntimeData.IsPlayerHasAllResourcesForAnyBuild())
            newContentMark.gameObject.SetActive(true);
        
        foreach (var res in SharedData.PlayerData.Resources)
            ResourcePanels[res.Key].gameObject.SetActive(false);
        
        getRewardAndGoToNextLevelButton.SetInteractable(GameUi.AdsService.IsRewardVideoReady());

        CheckTutorialCompleteState();
        StartCoroutine(ShowResourcePanels());
    }

    private IEnumerator ShowResourcePanels()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var res in SharedData.PlayerData.Resources)
        {
            ResourcePanels[res.Key].gameObject.SetActive(true);
            ResourcePanels[res.Key].Image.sprite = SharedData.StaticData.ResourcesData[res.Key].View.ItemSprite;
            ResourcePanels[res.Key].AmountText.text = $"{res.Value}";
            MinedResourcePanels[res.Key].text = $"+{SharedData.RuntimeData.MinedLevelResources[res.Key]}";
            MinedResourcePanels[res.Key].gameObject.SetActive(SharedData.RuntimeData.MinedLevelResources[res.Key] != 0);
        }
    }

    private void CheckTutorialCompleteState()
    {
        if (!SharedData.PlayerData.TutrorialStates[TutorialStep.UseItems])
        {
            getRewardAndGoToNextLevelButton.gameObject.SetActive(false);
            backToMetaButton.gameObject.SetActive(false);
            hideScreenButton.gameObject.SetActive(false);
        }
        else
        {
            getRewardAndGoToNextLevelButton.gameObject.SetActive(true);
            backToMetaButton.gameObject.SetActive(true);
            hideScreenButton.gameObject.SetActive(true);
        }

        if (SharedData.PlayerData.CurrentTutorialStep == TutorialStep.UseItems && !isTutorialScreen)
        {
            SetShowState(false);
        }
    }

    [Serializable]
    public class MinedResourcePanelByType : SerializedDictionary<ResourceType, TextMeshProUGUI>
    {
    }
}