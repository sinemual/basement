using System;

[Serializable]
public struct SoldierSaveData
{
    public int Level;
    public int WeaponLevel;
    public int ArmorLevel;
    public SoldierType SoldierType;
    public bool IsMutated;
}