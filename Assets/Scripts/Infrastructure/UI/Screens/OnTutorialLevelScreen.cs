using TMPro;
using UnityEngine;

public class OnTutorialLevelScreen : BaseScreen
{
    [SerializeField] private ActionButton openLevelCompleteScreenButton;
    [SerializeField] private TextMeshProUGUI levelGoalText;
    public ActionButton OpenLevelCompleteScreenButton => openLevelCompleteScreenButton;
    public TextMeshProUGUI LevelGoalText => levelGoalText;

    protected override void ManualStart()
    {
        openLevelCompleteScreenButton.OnClickEvent.AddListener(() =>
        {
            openLevelCompleteScreenButton.SetShowState(false);
            GameUi.EventBus.LevelGoalScreen.OnGoToTheNextLevelButtonTap();
            GameUi.EventBus.LevelCompleteScreen.OnStartNextLevelButton();
        });
    }
    
    public void UpdateLevelGoalText(string description)
    {
        LevelGoalText.text = $"{description.Replace("\\n", "\n")}";
    }
}