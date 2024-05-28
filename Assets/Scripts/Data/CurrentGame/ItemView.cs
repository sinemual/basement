using System;
using UnityEngine;

namespace Client.Data
{
    [Serializable]
    public class ItemView
    {
        public Sprite ItemSprite;
        public GameObject ItemPrefab;
        public GameObject DropItemPrefab;
        public GameObject UiDropItemPrefab;
    }
}