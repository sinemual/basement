using System.Collections.Generic;
using Client;
using Client.Data.Equip;
using Client.DevTools.MyTools;
using DG.Tweening;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetItemScreen : BaseScreen
{
    [SerializeField] private ActionButton getItemButton;
    [SerializeField] private Image getItemImage;
    [SerializeField] private TextMeshProUGUI getItemName;

    private ItemData currentItemData;
    protected override void ManualStart()
    {
        getItemButton.OnClickEvent.AddListener(GetItem);
    }
    
    private void SetItem(ItemData itemData)
    {
        currentItemData = itemData;
        getItemImage.sprite = currentItemData.View.ItemSprite;
        getItemName.text = currentItemData.Name;
    }

    private void GetItem()
    {
        GameUi.EventBus.GetItemScreen.OnGetItemButtonTap(currentItemData);
        SetShowState(false);
    }
}