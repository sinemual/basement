using System.Collections.Generic;
using Client.Data.Core;
using Client.Data.Equip;
using Sirenix.OdinInspector;
using UnityEngine;

public class FillLevelTool : MonoBehaviour
{
    [SerializeField] private List<ItemData> levelFill;
    [SerializeField] private List<EquipItemData> statFill;
    [SerializeField] private StatType statType;
    [SerializeField] private float baseStat;
    [SerializeField] private float coefStat;

    [Button]
    private void FillLevels()
    {
        int counter = 0;
        foreach (var item in levelFill)
        {
            item.Level = counter;
            counter++;
        }
    }
    
    [Button]
    private void FillStat()
    {
        statFill[0].Stats[statType]  = 0.0f;

        for (int i = 0; i < statFill.Count - 1; i++)
        {
            statFill[i + 1].Stats[statType] = baseStat * Mathf.Pow(coefStat, i);
        }
    }
    
    [Button]
    private void Copy()
    {
        foreach (var item in levelFill)
            statFill.Add(item as EquipItemData);
    }
    
    [Button]
    private void Clear()
    {
        levelFill = new List<ItemData>();
        statFill = new List<EquipItemData>();
    }
}