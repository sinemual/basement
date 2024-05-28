using Client.Data;
using UnityEngine;

public class OpenExitFromLevelScreen : BaseScreen
{
    [SerializeField] private ActionButton openExitFromLevelScreenButton;

    protected override void ManualStart()
    {
        openExitFromLevelScreenButton.OnClickEvent.AddListener(() =>
        {
            GameUi.ExitFromLevelScreen.SetShowState(true);
            GameUi.AnalyticService.LogEvent("open_level_exit_screen");
            SetShowState(false);
        });
    }
}