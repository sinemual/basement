using System;

namespace Client.Data.Equip
{
    [Serializable]
    public class ItemWithAmount
    {
        public ItemData ItemData;
        public int Amount;
    }
}