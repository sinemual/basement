using System;
using System.Globalization;
using Client.Data.Core;
using Data.Base;
using Leopotam.Ecs;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Client.Data.Equip
{
    [Serializable]
    public abstract class ItemData : BaseDataSO
    {
        public string Id;
        public int Level;
        public string Name;
        public string Description;
        public ItemView View;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            Id = name.ToLower().Replace(' ', '-');
            //SetDirty();
        }
        
        /*[Button]
       private void GetLevel()
       {
           int lvl = 0;
               Level = int.Parse(name, CultureInfo.InvariantCulture);
       }*/
#endif
    }
}