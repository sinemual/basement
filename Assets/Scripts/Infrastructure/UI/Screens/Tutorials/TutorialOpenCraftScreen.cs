using UnityEngine;

public class TutorialOpenCraftScreen : BaseScreen
{
    [SerializeField] private ActionButton actionButton;
    
    protected override void ManualStart()
    {
        actionButton.OnClickEvent.AddListener(() =>
        {
            GameUi.EventBus.GameScreen.OnOpenCraftScreenButtonTap();
        });
    }
}