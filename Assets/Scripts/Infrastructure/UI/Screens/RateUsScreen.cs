using UnityEngine;
using UnityEngine.UI;

public class RateUsScreen : BaseScreen
{
    [SerializeField] private Slider rateSlider;
    [SerializeField] private ActionButton sendRateToStoreButton;
    [SerializeField] private ActionButton hideScreenButton;

    protected override void ManualStart()
    {
        sendRateToStoreButton.OnClickEvent.AddListener(GoToTheStore);
        hideScreenButton.OnClickEvent.AddListener(() => SetShowState(false));
        OnShowScreen.AddListener(InitRateSlider);
    }

    private void InitRateSlider()
    {
        rateSlider.value = 5;
    }

    private void GoToTheStore()
    {
        if (rateSlider.value >= 4)
            Application.OpenURL("market://details?id=" + Application.identifier);
        SharedData.PlayerData.IsPlayerRatedGame = true;
        SetShowState(false);
    }
}