using Client.Data.Equip;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandItemScreen : BaseScreen
{
    [SerializeField] private ActionButton useItemButton;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAmountText;

    protected override void ManualStart()
    {
        useItemButton.OnPointerDownEvent.AddListener(GameUi.EventBus.CurrentHandItemScreen.OnUseItemButtonTap);
    }

    public Transform GetFlyPoint() => itemImage.gameObject.transform;

    public void SetItem(ItemData itemData)
    {
        itemAmountText.enabled = true;
        itemImage.enabled = true;
        itemImage.sprite = itemData.View.ItemSprite;
        itemAmountText.text = $"{SharedData.PlayerData.Inventory[itemData.Id]}";
    }

    public void TakeItem(ItemData itemData)
    {
        if (!SharedData.PlayerData.Inventory.ContainsKey(itemData.Id))
        {
            itemImage.enabled = false;
            itemAmountText.enabled = false;
        }
        else
            itemAmountText.text = $"{SharedData.PlayerData.Inventory[itemData.Id]}";
    }

    public void Impact()
    {
        itemImage.transform.DORewind();
        itemImage.transform.DOPunchScale(Vector3.one * 0.1f, 0.15f, 2, 0.5f);
    }
}