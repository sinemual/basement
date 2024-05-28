using System.Collections.Generic;
using Client;
using Client.Data.Equip;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using Client.DevTools.MyTools;
using UnityEngine.UI;

public class BuildScreen : BaseScreen
{
    [SerializeField] private ActionButton hideButton;
    [SerializeField] private ActionButton buildButton;
    [SerializeField] private ActionButton upgradeButton;
    [SerializeField] private List<ResourcePanel> neededResourcePanels;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private Image buildingImage;

    private BuildingData _currentData;

    protected override void ManualStart()
    {
        buildButton.OnClickEvent.AddListener(() =>
        {
            GameUi.EventBus.BuildScreen.OnBuildButtonTap(_currentData);
            SetShowState(false);
        });

        upgradeButton.OnClickEvent.AddListener(() =>
            {
                GameUi.EventBus.BuildScreen.OnUpgradeButtonTap(_currentData);
                SetShowState(false);
            }
        );

        GameUi.EventBus.BuildScreen.UpdateScreen += UpdateScreen;

        hideButton.OnClickEvent.AddListener(() => SetShowState(false));
    }

    public void UpdateScreen(BuildingData data)
    {
        _currentData = data;

        buildButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);

        int currentLevel = SharedData.PlayerData.BuildingsSaveData[data.Type].CurrentLevel;
        if (currentLevel > 0)
        {
            upgradeButton.gameObject.SetActive(true);
            upgradeButton.SetInteractable(SharedData.RuntimeData.IsPlayerHasAllResourcesForBuild(data));
        }
        else
        {
            buildButton.gameObject.SetActive(true);
            buildButton.SetInteractable(SharedData.RuntimeData.IsPlayerHasAllResourcesForBuild(data));
        }

        buildingNameText.text = $"{data.Name}";
        buildingImage.sprite = data.ViewSprite;
        levelText.text = $"LEVEL {SharedData.PlayerData.BuildingsSaveData[data.Type].CurrentLevel + 2}";

        foreach (var panel in neededResourcePanels)
            panel.gameObject.SetActive(false);

        int counter = 0;
        foreach (var item in data.NeededItems)
        {
            neededResourcePanels[counter].gameObject.SetActive(true);
            neededResourcePanels[counter].Image.sprite = item.ItemData.View.ItemSprite;
            neededResourcePanels[counter].AmountText.text = $"{item.Amount}";
            counter++;
        }
    }
}