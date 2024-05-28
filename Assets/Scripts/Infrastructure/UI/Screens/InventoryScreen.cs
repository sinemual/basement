using System.Collections.Generic;
using System.Linq;
using Client.Data.Equip;
using Client.DevTools.MyTools;
using Leopotam.Ecs;
using UnityEngine;

public class InventoryScreen : BaseScreen
{
    [SerializeField] private ActionButton hideScreenButton;
    [SerializeField] private List<InventorySlot> items;

    protected override void ManualStart()
    {
        hideScreenButton.OnClickEvent.AddListener(() => SetShowState(false));
        OnShowScreen.AddListener(SetItems);
    }

    private void SetItems()
    {
        foreach (var slot in items) 
        {
            slot.Image.gameObject.SetActive(false);
            slot.AmountText.gameObject.SetActive(false);
        }
        
        int counter = 0;
        foreach (var item in SharedData.PlayerData.Inventory)
        {
            items[counter].Image.gameObject.SetActive(true);
            items[counter].AmountText.gameObject.SetActive(true);
            
            items[counter].Item = SharedData.StaticData.ItemDatabase.First(x => x.Id == item.Key);
            items[counter].Image.sprite = items[counter].Item.View.ItemSprite;
            items[counter].AmountText.text = $"{item.Value}";
            items[counter].Button.OnClickEvent.RemoveAllListeners();
            var buttonTempCounter = counter;
            items[counter].Button.OnClickEvent.AddListener(() => ChooseItem(items[buttonTempCounter].Item));
            counter++;
        }
    }

    private void ChooseItem(ItemData item)
    {
        GameUi.EventBus.InventoryScreen.OnChooseItemButtonTap(item);
    }
}