using Client.Data;
using UnityEngine;

public class OpenGameProgressScreen : BaseScreen
{
    [SerializeField] private ActionButton showGameProgressScreenButton;

    protected override void ManualStart()
    {
        showGameProgressScreenButton.OnClickEvent.AddListener(() =>
        {
            GameUi.AudioService.Play(Sounds.UiClickSound);
           // GameUi.GameProgressScreen.SetShowState(true);
            GameUi.EventBus.OpenGameProgressScreen.OnGameProgressScreenButtonTap();
        });
    }
}