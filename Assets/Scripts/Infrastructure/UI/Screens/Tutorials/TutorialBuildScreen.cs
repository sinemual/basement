using System.Collections.Generic;
using Client;
using Client.Data.Equip;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBuildScreen : BaseScreen
{
    [SerializeField] private ActionButton hideButton;
    [SerializeField] private ActionButton buildButton;
    [SerializeField] private List<ResourcePanel> neededResourcePanels;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private Image buildingImage;
    
    protected override void ManualStart()
    {
        buildButton.OnClickEvent.AddListener(() =>
            {
                GameUi.EventBus.BuildScreen.OnBuildButtonTap(SharedData.StaticData.BuildingsData[BuildingType.Lumberjack].Value[0]);
                SetShowState(false);
                GameUi.BuildScreen.SetShowState(false);
            }
        );
        
        OnShowScreen.AddListener(UpdateScreen);
    }
    
    private void UpdateScreen()
    {
        var data = SharedData.StaticData.BuildingsData[BuildingType.Lumberjack].Value[0];
        
        buildingNameText.text = $"{data.Name}";
        buildingImage.sprite = data.ViewSprite;
        
        foreach (var panel in neededResourcePanels)
            panel.gameObject.SetActive(false);

        int currentLevel = SharedData.PlayerData.BuildingsSaveData[data.Type].CurrentLevel;
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