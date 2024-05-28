using System;
using Client;
using Client.Data.Equip;
using DG.Tweening;
using UnityEngine;

public class CraftScreen : BaseScreen
{
    [SerializeField] private ActionButton hideScreenButton;
    [SerializeField] private CraftEquipPanel craftPanels;
    [SerializeField] private AdsUpgradeButtonByType adsUpgradeButtonByType;

    protected override void ManualStart()
    {
        hideScreenButton.OnClickEvent.AddListener(() =>
        {
            GameUi.EventBus.CraftScreen.OnCloseCraftScreenButtonTap();
            SetShowState(false);
        });

        GameUi.EventBus.Resources.ChangeResourceAmount += UpdateRecipePanels;
        OnShowScreen.AddListener(UpdateRecipePanels);
    }

    private void UpdateRecipePanels()
    {
        SharedData.RuntimeData.PlayerOpenCraftScreen();

        int counter = 0;
        foreach (var equip in SharedData.PlayerData.Equipment)
        {
            craftPanels[equip.Key].CraftButton.SetInteractable(false);
            craftPanels[equip.Key].MaxLevelPanel.gameObject.SetActive(false);

            if (equip.Value + 1 < SharedData.StaticData.AllEquip[equip.Key].Value.Count)
            {
                craftPanels[equip.Key].SetCraftRecipeData(SharedData.StaticData.EquipRecipes[equip.Key].Value[equip.Value + 1]);
                craftPanels[equip.Key].SetCraftRecipeView(SharedData.StaticData.EquipRecipes[equip.Key].Value[equip.Value]);
                craftPanels[equip.Key].CraftButton
                    .SetInteractable(SharedData.RuntimeData.IsPlayerHasAllResourcesForCraft(craftPanels[equip.Key].RecipeData));
                craftPanels[equip.Key].CraftButton.OnClickEvent.RemoveAllListeners();
                craftPanels[equip.Key].CraftButton.OnClickEvent.AddListener(() => CraftItem(craftPanels[equip.Key].RecipeData));
            }
            else
            {
                adsUpgradeButtonByType[equip.Key].gameObject.SetActive(false);
                craftPanels[equip.Key].MaxLevelPanel.gameObject.SetActive(true);
            }

            counter++;
        }
    }

    private void CraftItem(CraftRecipeData craftRecipeData)
    {
        GameUi.EventBus.CraftScreen.OnCraftButtonTap(craftRecipeData);
        UpdateRecipePanels();

        if (craftRecipeData.GettedItem is EquipItemData item)
        {
            Impact(item.Type);
            DisableRewardAvailableButton(item.Type);
        }
    }

    public void SetRewardAvailableButton(EquipType equipType)
    {
        foreach (var equip in SharedData.PlayerData.Equipment)
            adsUpgradeButtonByType[equip.Key].gameObject.SetActive(false);

        adsUpgradeButtonByType[equipType].gameObject.SetActive(true);
        adsUpgradeButtonByType[equipType].SetInteractable(GameUi.AdsService.IsRewardVideoReady());
        adsUpgradeButtonByType[equipType].OnClickEvent.AddListener(
#if UNITY_EDITOR
            () => CraftItem(craftPanels[equipType].RecipeData)
#else
                    () =>
            GameUi.AdsService.ShowRewardVideo($"ads_tool_{equipType}_upgrade", 
                () => CraftItem(craftPanels[equipType].RecipeData))
#endif

        );
    }

    public void DisableRewardAvailableButton(EquipType equipType)
    {
        Debug.Log($"DisableRewardAvailableButton {equipType}");
        adsUpgradeButtonByType[equipType].gameObject.SetActive(false);
    }

    private void Impact(EquipType type)
    {
        craftPanels[type].Panel.transform.DORewind();
        craftPanels[type].Panel.transform.DOPunchScale(Vector3.one * 0.1f, 0.15f, 2, 0.5f);
    }

    [Serializable]
    public class CraftEquipPanel : SerializedDictionary<EquipType, CraftItemsPanel>
    {
    }

    [Serializable]
    public class AdsUpgradeButtonByType : SerializedDictionary<EquipType, ActionButton>
    {
    }
}