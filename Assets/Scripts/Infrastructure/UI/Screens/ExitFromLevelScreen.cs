using UnityEngine;

public class ExitFromLevelScreen : BaseScreen
{
    [SerializeField] private ActionButton exitFromLevelButton;
    [SerializeField] private ActionButton continueLevelButton;

    protected override void ManualStart()
    {
        exitFromLevelButton.OnClickEvent.AddListener(() =>
        {
            GameUi.EventBus.ExitFromLevelScreen.OnExitFromLevelButtonTap();
            SetShowState(false);
            
        });
        
        continueLevelButton.OnClickEvent.AddListener(() =>
        {
            GameUi.OpenExitFromLevelScreen.SetShowState(true);
            SetShowState(false);
        });
    }
}