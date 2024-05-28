using Client.Data.Equip;
using UnityEngine;

namespace Client.ECS.CurrentGame.Loot.Systems
{
    public struct UiGoToPlayerRequest
    {
        public ItemData ItemData;
        public GameObject ItemGo;
    }
}