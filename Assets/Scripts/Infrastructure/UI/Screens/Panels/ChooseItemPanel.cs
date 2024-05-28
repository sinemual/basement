using System.Collections;
using System.Collections.Generic;
using Client.Data.Equip;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class ChooseItemPanel : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private ActionButton chooseButton;
        [SerializeField] private Image itemImage;

        private ItemData _itemData;
        
        public ActionButton ChooseButton => chooseButton;

        public ItemData ItemData => _itemData;

        public GameObject Panel => panel;

        public void SetItemData(ItemData itemData)
        {
            _itemData = itemData;
            itemImage.sprite = ItemData.View.ItemSprite;
        }
    }
}