using Client.Data;
using UnityEngine;

public class OpenLevelCompleteScreen : BaseScreen
{
    [SerializeField] private ActionButton showLevelCompleteScreenButton;

    protected override void ManualStart()
    {
        showLevelCompleteScreenButton.OnClickEvent.AddListener(() =>
        {
            GameUi.AudioService.Play(Sounds.UiClickSound);
            GameUi.LevelCompleteScreen.SetShowState(true);
            SharedData.RuntimeData.CurrentGameState = GameState.LevelComplete;
        });
    }
}