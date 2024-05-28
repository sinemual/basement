using Client.Data.Core;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Data.Base
{
    public abstract class BaseDataSO : ScriptableObject
    {
        protected SharedData SharedData;

#if UNITY_EDITOR
        [Button]
        public new void SetDirty()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        [Button]
        public void Log()
        {
            var json = JsonUtility.ToJson(this);
            Debug.Log(json);
        }
#endif
    }
}