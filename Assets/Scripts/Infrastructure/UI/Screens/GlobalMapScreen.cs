using Client.Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalMapScreen : BaseScreen
{
    [SerializeField] private ActionButton goToVillageButton;
    [SerializeField] private ActionButton startNextLevelButton;

    public ActionButton GoToVillageButton => goToVillageButton;

    protected override void ManualStart()
    {
        goToVillageButton.OnClickEvent.AddListener(() =>
        {
            GameUi.EventBus.GlobalMapScreen.OnGoToVillageButton();
            SetShowState(false);
        });

        startNextLevelButton.OnClickEvent.AddListener(() =>
        {
            GameUi.EventBus.GlobalMapScreen.OnStartNextLevelButton();
            SetShowState(false);
        });
    }
}