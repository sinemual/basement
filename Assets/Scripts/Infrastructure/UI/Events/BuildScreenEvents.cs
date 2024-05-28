using System;
using Client.Data.Equip;

public class BuildScreenEvents
{
    public event Action<BuildingData> BuildButtonTap;
    public void OnBuildButtonTap(BuildingData data) => BuildButtonTap?.Invoke(data);
    
    public event Action<BuildingData> UpgradeButtonTap;
    public void OnUpgradeButtonTap(BuildingData data) => UpgradeButtonTap?.Invoke(data);
    
    public event Action<BuildingData> UpdateScreen;
    public void OnUpdateScreen(BuildingData data) => UpdateScreen?.Invoke(data);
}