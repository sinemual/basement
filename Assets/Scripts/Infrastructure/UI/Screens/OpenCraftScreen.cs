using Client.Data;
using UnityEngine;

public class OpenCraftScreen : BaseScreen
{
    [SerializeField] private ActionButton showCraftScreenButton;
    [SerializeField] private GameObject newUpgradeAvailableMark;

    protected override void ManualStart()
    {
        showCraftScreenButton.OnClickEvent.AddListener(() =>
        {
            GameUi.AudioService.Play(Sounds.UiClickSound);
            GameUi.CraftScreen.SetShowState(true);
        });
    }

    
    public Transform GetCraftFlyPoint() => showCraftScreenButton.gameObject.transform;

}