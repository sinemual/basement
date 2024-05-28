using System.IO;
using Client.Data.Core;
using Client.DevTools.MyTools;
using UnityEngine;

namespace Data.Base
{
    public abstract class BaseData
    {
        protected SharedData SharedData;
        public abstract void ResetData();

        public void SaveData()
        {
            var json = JsonUtility.ToJson(this);
            Debug.Log(json);
            var filename = $"{GetType()}.json";
            var dataPath = Path.Combine(Utility.GetDataPath(), filename);
            if (!Directory.Exists(Utility.GetDataPath()))
                Directory.CreateDirectory(Utility.GetDataPath());
            File.WriteAllText(dataPath, json);
        }

        public void LoadData()
        {
            var filename = $"{GetType()}.json";
            var dataPath = Path.Combine(Utility.GetDataPath(), filename);
            if (File.Exists(dataPath))
            {
                var json = File.ReadAllText(dataPath);
                Debug.Log(json);
                JsonUtility.FromJsonOverwrite(json, this);
            }
        }
    }
}