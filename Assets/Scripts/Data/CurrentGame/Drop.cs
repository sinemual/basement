using System;
using Client.Data.Equip;

namespace Client.Data
{
    [Serializable]
    public class Drop
    {
        public ItemData ItemData;
        public int Amount;
        public int Chance;
    }
}
