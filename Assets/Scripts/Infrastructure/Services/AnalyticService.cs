using System.Collections.Generic;
using Client.Data.Core;

public class AnalyticService
{
    private SharedData _data;

    public AnalyticService(SharedData data) => _data = data;

    public void LogEvent(string evnt)
    {
        var _params = new Dictionary<string, object>();
        _params.Add("map_num", $"{_data.PlayerData.CurrentWarStepIndex}");
        _params.Add("location", $"{_data.RuntimeData.CurrentLocationType}");

        string resourcesString = "";
        foreach (var res in _data.PlayerData.Resources)
            resourcesString += $"{res.Key}: {res.Value} \n";

        string equipmentString = "";
        foreach (var equip in _data.PlayerData.Equipment)
            equipmentString += $"{equip.Key}: {equip.Value}\n";

        _params.Add("resources", $"{resourcesString}");
        _params.Add("equipment", $"{equipmentString}");
    }

    public void LogEventWithParameter(string evnt, string parameter)
    {
        var _params = new Dictionary<string, object>();
        _params.Add(evnt, parameter);
        _params.Add("map_num", $"{_data.PlayerData.CurrentWarStepIndex}");
        _params.Add("location", $"{_data.RuntimeData.CurrentLocationType}");

        string resourcesString = "";
        foreach (var res in _data.PlayerData.Resources)
            resourcesString += $"{res.Key}: {res.Value} \n";

        string equipmentString = "";
        foreach (var equip in _data.PlayerData.Equipment)
            equipmentString += $"{equip.Key}: {equip.Value} \n";

        _params.Add("resources", $"{resourcesString}");
        _params.Add("equipment", $"{equipmentString}");
    }
    
    public void LogAdsEvent(AdsService.AdsType adsType, string placement)
    {
        var _params = new Dictionary<string, object>();
        _params.Add("ads_show", placement);
        _params.Add("ads_type", adsType);
    }
}