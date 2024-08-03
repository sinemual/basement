using Client.Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnLevelScreen : BaseScreen
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image levelProgressBarFill;
    [SerializeField] private TextMeshProUGUI levelProgressBarText;
    [SerializeField] private GameObject experiencePanel;

    protected override void ManualStart()
    {
        GameUi.EventBus.Experience.GetExperience += UpdateProgressBar;
        OnShowScreen.AddListener(PrepareScreen);
    }
    
    private void PrepareScreen()
    {
        levelText.text = $"LEVEL {SharedData.PlayerData.CurrentWarStepIndex + 1}";
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        levelProgressBarFill.fillAmount = (float)SharedData.RuntimeData.CurrentLevelExperience / (float)SharedData.RuntimeData.NeededLevelExperience;
        levelProgressBarText.text = $"{(int)SharedData.RuntimeData.CurrentLevelExperience} / {(int)SharedData.RuntimeData.NeededLevelExperience}";
        ExperienceImpact();
    }

    public Transform GetExperienceFlyPoint() => levelProgressBarFill.gameObject.transform;

    private void ExperienceImpact()
    {
        experiencePanel.gameObject.transform.DORewind();
        experiencePanel.gameObject.transform.DOPunchScale(Vector3.one * 0.05f, 0.15f, 2, 0.5f);
    }
}