using System;
using Client;
using Client.Data.Equip;
using DG.Tweening;
using UnityEngine;

public class ResourceScreen : BaseScreen
{
    [SerializeField] private ResourcePanelByType resourcePanels;

    protected override void ManualStart()
    {
        GameUi.EventBus.Resources.ChangeResourceAmount += SetResource;
        OnShowScreen.AddListener(SetResource);
    }

    private void SetResource()
    {
        foreach (var res in SharedData.PlayerData.Resources)
        {
            resourcePanels[res.Key].Image.sprite = SharedData.StaticData.ResourcesData[res.Key].View.ItemSprite;
            resourcePanels[res.Key].AmountText.text = $"{SharedData.PlayerData.Resources[res.Key]}";
        }
    }

    public Transform GetFlyPoint(ResourceType type)
    {
        return resourcePanels[type].gameObject.transform;
    }

    public void Impact(ResourceType type)
    {
        if (!DOTween.IsTweening(resourcePanels[type].gameObject.transform))
        {
            resourcePanels[type].gameObject.transform.DORewind();
            resourcePanels[type].gameObject.transform.DOPunchScale(Vector3.one * 0.5f, 0.15f, 2, 0.5f);
        }
    }

    [Serializable]
    public class ResourcePanelByType : SerializedDictionary<ResourceType, ResourcePanel>
    {
    }
}