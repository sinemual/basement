using System.Collections;
using TMPro;
using UnityEngine;

public class NoAdsIapScreen : BaseScreen
{
    [SerializeField] private ActionButton buyNoAdsIapButton;
    [SerializeField] private ActionButton hideScreenButton;
    [SerializeField] private TextMeshProUGUI priceText;

    protected override void ManualStart()
    {
        buyNoAdsIapButton.OnClickEvent.AddListener(() =>
        {
            GameUi.EventBus.NoAdsIapScreen.OnBuyNoAdsIapButtonTap();
            SetShowState(false);
        });

        hideScreenButton.OnClickEvent.AddListener(() => SetShowState(false));
        
        OnShowScreen.AddListener(UpdatePrice);
    }

    private void UpdatePrice()
    {
        /*priceText.text = $"{PurchaseController.instance.GetPrice("one_week_subscription")}";*/
    }

    private IEnumerator ShowHideButtonWithDelay(float sec)
    {
        yield return new WaitForSeconds(sec);
        hideScreenButton.SetShowState(true);
    }
}