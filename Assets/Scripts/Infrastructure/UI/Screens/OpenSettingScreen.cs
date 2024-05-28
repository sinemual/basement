using Client.Data;
using Client.DevTools.MyTools;
using Data;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;
using UnityEngine;

public class OpenSettingScreen : BaseScreen
{
    [SerializeField] private ActionButton showSettingScreenButton;

    protected override void ManualStart()
    {
        showSettingScreenButton.OnClickEvent.AddListener(() =>
        {
            GameUi.AudioService.Play(Sounds.UiClickSound);
            GameUi.SettingScreen.SetShowState(!GameUi.SettingScreen.ScreenIsShow);
        });
    }
}