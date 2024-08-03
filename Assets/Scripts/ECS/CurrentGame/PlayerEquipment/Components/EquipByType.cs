using System;
using Client.Data.Equip;

namespace Client.ECS.CurrentGame.PlayerEquipment.Components
{
    [Serializable]
    public class EquipByType : SerializedDictionary<PlayerEquipType, EquipItemData>
    {
    }
}