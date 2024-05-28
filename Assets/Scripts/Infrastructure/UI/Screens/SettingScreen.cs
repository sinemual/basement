using Client;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

public class SettingScreen : BaseScreen
{
    [Header("Vibration")] [SerializeField] private ActionButton vibrationButton;

    [SerializeField] private Sprite vibrationOnSprite;
    [SerializeField] private Sprite vibrationOffSprite;
    [SerializeField] private Image vibrationStateImage;

    [Header("Sound")] [SerializeField] private ActionButton soundButton;

    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private Image soundStateImage;

    protected override void ManualStart()
    {
        OnShowScreen.AddListener(UpdateSprites);

        vibrationButton.OnClickEvent.AddListener(VibrationButtonTap);
        soundButton.OnClickEvent.AddListener(SoundButtonTap);
    }

    public void UpdateSprites()
    {
        vibrationStateImage.sprite = SharedData.PlayerData.IsVibrationOn ? vibrationOnSprite : vibrationOffSprite;
        soundStateImage.sprite = SharedData.PlayerData.IsSoundOn ? soundOnSprite : soundOffSprite;
    }

    private void VibrationButtonTap()
    {
        GameUi.EventBus.SettingScreen.OnVibrationTriggerButtonTap();
        vibrationStateImage.sprite = SharedData.PlayerData.IsVibrationOn ? vibrationOffSprite : vibrationOnSprite;
        SetShowState(false);
    }

    private void SoundButtonTap()
    {
        GameUi.EventBus.SettingScreen.OnSoundTriggerButtonTap();
        soundStateImage.sprite = SharedData.PlayerData.IsSoundOn ? soundOffSprite : soundOnSprite;
        SetShowState(false);
    }
}