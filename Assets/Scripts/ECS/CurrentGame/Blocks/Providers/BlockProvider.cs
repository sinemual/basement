using System;
using Client.Data;
using Client.Data.Equip;
using UnityEngine;

[Serializable]
public struct BlockProvider
{
    public BlockType Type;
    public int Level;
    public GameObject Model;
    public EquipType MineEquipType;
}