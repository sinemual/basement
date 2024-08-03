using System;
using Client;
using Client.Data.Equip;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChangeEquipScreen : BaseScreen
{
    [SerializeField] private ActionButton changeButton;
    [SerializeField] private Image bowImage;
    [SerializeField] private Image pickaxeImage;

    protected override void ManualStart()
    {
        changeButton.OnClickEvent.AddListener(ChangeItem);
        GameUi.EventBus.ChangeItemScreen.ChangeItem += OnChangeItem;
    }

    private void ChangeItem()
    {
        GameUi.EventBus.ChangeItemScreen.OnChangeItemButtonTap();
        Impact();
    }

    private void OnChangeItem(PlayerEquipType type)
    {
        if (type == PlayerEquipType.Bow)
        {
            bowImage.rectTransform.localScale *= 1.5f;
            pickaxeImage.rectTransform.localScale = Vector3.one;
            pickaxeImage.transform.SetParent(pickaxeImage.transform.parent);
        }
        else
        {
            pickaxeImage.rectTransform.localScale *= 1.5f;
            bowImage.rectTransform.localScale = Vector3.one;
            bowImage.transform.SetParent(bowImage.transform.parent);
        }
    }

    private void Impact()
    {
        changeButton.transform.DORewind();
        changeButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.15f, 2, 0.5f);
    }
}