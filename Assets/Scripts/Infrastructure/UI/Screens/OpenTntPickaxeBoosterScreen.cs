using Client.Data.Equip;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OpenTntPickaxeBoosterScreen : BaseScreen
{
    [SerializeField] private ActionButton openInventoryScreenButton;

    protected override void ManualStart()
    {
        openInventoryScreenButton.OnClickEvent.AddListener(() =>
        {
            GameUi.TntPickaxeBoosterScreen.SetShowState(true);
            SetShowState(false);
        });
    }
}