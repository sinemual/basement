using System;
using Client;
using Client.Data.Equip;
using DG.Tweening;
using UnityEngine;

public class ChooseItemScreen : BaseScreen
{
    [SerializeField] private ChooseItemEquipPanel itemPanels;

    protected override void ManualStart()
    {
        OnShowScreen.AddListener(UpdateItemPanels);
    }

    private void UpdateItemPanels()
    {
        int counter = 0;
        foreach (var equip in SharedData.PlayerData.Equipment)
        {
            itemPanels[equip.Key].SetItemData(SharedData.StaticData.EquipRecipes[equip.Key].Value[equip.Value].GettedItem);
            itemPanels[equip.Key].ChooseButton.OnClickEvent.RemoveAllListeners();
            itemPanels[equip.Key].ChooseButton.OnClickEvent.AddListener(() => ChooseItem(itemPanels[equip.Key].ItemData));
            counter++;
        }
    }

    private void ChooseItem(ItemData itemData)
    {
        GameUi.EventBus.ChooseItemScreen.OnChooseItemButtonTap(itemData);
        UpdateItemPanels();
        if (itemData is EquipItemData equip)
            Impact(equip.Type);
    }

    private void Impact(EquipType type)
    {
        itemPanels[type].Panel.transform.DORewind();
        itemPanels[type].Panel.transform.DOPunchScale(Vector3.one * 0.1f, 0.15f, 2, 0.5f);
    }

    [Serializable]
    public class ChooseItemEquipPanel : SerializedDictionary<EquipType, ChooseItemPanel>
    {
    }
}