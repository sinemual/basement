using UnityEngine;

public class OpenNoAdsIapScreen : BaseScreen
{
    [SerializeField] private ActionButton openNoAdsIapScreenButton;

    protected override void ManualStart()
    {
        openNoAdsIapScreenButton.OnClickEvent.AddListener(() =>
        {
            GameUi.NoAdsIapScreen.SetShowState(true);
            SetShowState(false);
        });
    }
}