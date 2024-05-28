using System;
using Client.Data.Equip;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class InventorySlot : MonoBehaviour
{
    public ActionButton Button;
    public Image Image;
    public TextMeshProUGUI AmountText;
    [HideInInspector] public ItemData Item;
}