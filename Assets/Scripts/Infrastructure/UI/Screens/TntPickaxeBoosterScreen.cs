using Client.Data.Equip;
using UnityEngine;

public class TntPickaxeBoosterScreen : BaseScreen
{
    [SerializeField] private ActionButton hideScreenButton;
    [SerializeField] private ActionButton rewardActivateBoosterButton;

    protected override void ManualStart()
    {
        hideScreenButton.OnClickEvent.AddListener(() => SetShowState(false));
        rewardActivateBoosterButton.OnClickEvent.AddListener(ActivateBooster);
        
        OnShowScreen.AddListener(UpdateScreen);
    }

    private void UpdateScreen()
    {
        rewardActivateBoosterButton.SetInteractable(GameUi.AdsService.IsRewardVideoReady());
    }

    private void ActivateBooster()
    {
        GameUi.EventBus.TntPickaxeBoosterScreen.OnActivateTntPickaxeBoosterButtonTap();
        SetShowState(false);
    }
}