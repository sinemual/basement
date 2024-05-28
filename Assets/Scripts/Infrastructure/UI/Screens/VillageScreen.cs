using Client.Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VillageScreen : BaseScreen
{
    [SerializeField] private ActionButton goToGlobalScreenButton;
    public ActionButton GoToGlobalScreenButton => goToGlobalScreenButton;

    protected override void ManualStart()
    {
        goToGlobalScreenButton.OnClickEvent.AddListener(() =>
        {
            SetShowState(false);
            
            GameUi.EventBus.VillageScreen.OnGoToGlobalMapButtonTap();
        });
    }
}