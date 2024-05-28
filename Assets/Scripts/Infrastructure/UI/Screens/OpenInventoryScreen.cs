using Client.Data.Equip;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OpenInventoryScreen : BaseScreen
{
    [SerializeField] private ActionButton openInventoryScreenButton;
    [SerializeField] private Image openInventoryScreenButtonImage;

    protected override void ManualStart()
    {
        openInventoryScreenButton.OnClickEvent.AddListener(() => {GameUi.InventoryScreen.SetShowState(true);});
    }

    public Transform GetFlyPoint()
    {
        return openInventoryScreenButtonImage.gameObject.transform;
    }
    
    public void Impact()
    {
        openInventoryScreenButtonImage.gameObject.transform.DORewind(); 
        openInventoryScreenButtonImage.gameObject.transform.DOPunchScale(Vector3.one * 0.2f, 0.15f, 2, 0.5f);
    }
}