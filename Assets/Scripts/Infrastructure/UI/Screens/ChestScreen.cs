using System.Collections;
using System.Collections.Generic;
using Client;
using Client.Data;
using Client.Data.Equip;
using Client.DevTools.MyTools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestScreen : BaseScreen
{
    [SerializeField] private ActionButton openChestButton;
    [SerializeField] private ActionButton continueButton;
    [SerializeField] private Image chestImage;
    [SerializeField] private Animator chestAnimator;
    [SerializeField] private TextMeshProUGUI chestName;
    
    private ItemData currentItemData;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    protected override void ManualStart()
    {
        Debug.Log($"{continueButton}");
        openChestButton.OnClickEvent.AddListener(OpenChest);
        continueButton.OnClickEvent.AddListener(() => SetShowState(false));

        OnShowScreen.AddListener(PrepareScreen);
    }

    private void PrepareScreen()
    {
        Utility.Animation.ResetAnimator(chestAnimator);
        openChestButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);
    }

    public Transform GetFlyPoint() => chestImage.gameObject.transform;

    public void SetChestData(ItemData itemData)
    {
        currentItemData = itemData;
        //chestName.text = currentItemData.Name;
    }

    private void OpenChest()
    {
        GameUi.EventBus.OpenChestScreen.OnOpenChestButtonTap(currentItemData);
        chestAnimator.SetTrigger(IsOpen);
        StartCoroutine(ShowContinueButton());
    }
    
    private IEnumerator ShowContinueButton()
    {
        yield return new WaitForSeconds(0.5f);
        openChestButton.SetShowState(false);
        continueButton.gameObject.SetActive(true);
    }

}