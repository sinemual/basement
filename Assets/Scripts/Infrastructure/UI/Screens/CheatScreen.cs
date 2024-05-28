using Client;
using Leopotam.Ecs;
using UnityEngine;

public class CheatScreen : BaseScreen
{
    [SerializeField] private ActionButton getMoneyButton;
    [SerializeField] private ActionButton goToMidGameButton;
    [SerializeField] private ActionButton goToLateGameButton;
    [SerializeField] private ActionButton resetPlayerDataButton;
    [SerializeField] private ActionButton hideScreenButton;

    protected override void ManualStart()
    {
        getMoneyButton.OnClickEvent.AddListener(() => CheatGetMoney());
        goToMidGameButton.OnClickEvent.AddListener(() => CheatGoToMidGame());
        goToLateGameButton.OnClickEvent.AddListener(() => CheatGoToLateGame());
        resetPlayerDataButton.OnClickEvent.AddListener(() => CheatResetPlayerData());
        hideScreenButton.OnClickEvent.AddListener(() => SetShowState(false));
    }

    private void CheatGetMoney()
    {
    }

    private void CheatGoToMidGame()
    {
    }

    private void CheatGoToLateGame()
    {
    }

    private void CheatResetPlayerData()
    {
        SharedData.PlayerData.ResetData();
        PlayerPrefs.DeleteAll();
    }
}