using System;
using Client.Data.Core;

namespace Client.Data.Equip
{
    [Serializable]
    public class StatValue : SerializedDictionary<StatType, float>
    {
    }
}