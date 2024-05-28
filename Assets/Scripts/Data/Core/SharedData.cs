    using System.IO;
using Client.DevTools.MyTools;
using Data;
using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;

namespace Client.Data.Core
{
    public class SharedData : MonoBehaviour
    {
        public StaticData StaticData;
        public BalanceData BalanceData;
        public PlayerData PlayerData;
        public RuntimeData RuntimeData;
        public SceneData SceneData;
        public AudioData AudioData;
        
        [Header("Ads Data")]
        public InterstitialSettingsData InterstitialSettingsData;
        public RewardEquipUpgradeData RewardEquipUpgradeData;
        public RateUsData RateUsData;

        public void ManualStart()
        {
            Debug.Log(Utility.GetDataPath());

            RuntimeData = new RuntimeData();

            RuntimeData.InjectData(this);
            PlayerData.InjectData(this);
            BalanceData.InjectData(this);
            StaticData.ResetData();

#if UNITY_EDITOR
            LoadData();
#else
            if (PlayerPrefs.GetInt("IsThisVersionDataLaunchedBefore", 0) == 1)
                LoadData();
#endif
        }

        private void SaveData()
        {
            PlayerData.IsGameLaunchedBefore = true;
            PlayerPrefs.SetInt("IsThisVersionDataLaunchedBefore", 1);
            PlayerData.SaveData();
        }

        private void LoadData()
        {
            PlayerData.LoadData();
        }

        [ExecuteInEditMode]
        public void ResetData()
        {
            PlayerData.ResetData();
        }

#if UNITY_EDITOR
        [ExecuteInEditMode]
        [MenuItem("Tools/DeleteAllGameData")]
        public static void DeleteAllGameData()
        {
            if (Directory.Exists(Utility.GetDataPath()))
                Directory.Delete(Utility.GetDataPath(), true);
        }
#endif
        private void OnApplicationPause(bool pause)
        {
            if (pause)
                SaveData();
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }
    }
}