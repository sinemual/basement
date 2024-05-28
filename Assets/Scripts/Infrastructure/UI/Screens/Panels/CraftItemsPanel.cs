using System.Collections;
using System.Collections.Generic;
using Client.Data.Equip;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class CraftItemsPanel : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private ActionButton craftButton;
        [SerializeField] private Image craftItemImage;
        [SerializeField] private Image levelProgressBarImage;
        [SerializeField] private List<ResourcePanel> neededResources;
        [SerializeField] private GameObject maxLevelPanel;

        private CraftRecipeData _craftRecipeData;

        public ActionButton CraftButton => craftButton;
        public GameObject MaxLevelPanel => maxLevelPanel;
        public GameObject Panel => panel;

        public CraftRecipeData RecipeData
        {
            get => _craftRecipeData;
            set => _craftRecipeData = value;
        }

        public void SetCraftRecipeData(CraftRecipeData craftRecipeData)
        {
            RecipeData = craftRecipeData;
            //craftItemImage.sprite = RecipeData.GettedItem.View.ItemSprite;

            foreach (var neededResource in neededResources)
                neededResource.gameObject.SetActive(false);

            for (int i = 0; i < RecipeData.NeededItems.Count; i++)
            {
                neededResources[i].gameObject.SetActive(true);
                neededResources[i].Image.sprite = RecipeData.NeededItems[i].ItemData.View.ItemSprite;
                neededResources[i].AmountText.text = $"{RecipeData.NeededItems[i].Amount}";
            }
        }

        public void SetCraftRecipeView(CraftRecipeData craftRecipeData)
        {
            craftItemImage.sprite = craftRecipeData.GettedItem.View.ItemSprite;
            levelProgressBarImage.fillAmount = craftRecipeData.Level / 5.0f;
        }
    }
}