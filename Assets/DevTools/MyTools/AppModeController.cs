using System.Collections.Generic;
using Cinemachine;
using Client.Data.Core;
using Client.DevTools.MyTools;
using Sirenix.OdinInspector;
/*#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif*/
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class AppModeController : MonoBehaviour/*, IPreprocessBuildWithReport*/
    {
        public AppMode AppMode;

        public SharedData Data;
        
        public List<GameObject> ForRelease;
        public List<GameObject> ForRecording;
        public List<Image> ForRecordingImages;
        public List<GameObject> ForDebug;
        
/*#if UNITY_EDITOR
        public int callbackOrder { get; }
        public void OnPreprocessBuild(BuildReport report)
        {
            if (EditorUserBuildSettings.buildAppBundle)
                AppMode = AppMode.Release;
        }
#endif*/
        
        void Awake()
        {
            if (AppMode == AppMode.Release)
            {
                foreach (var go in ForDebug)
                    go.SetActive(false);

                foreach (var go in ForRecording)
                    go.SetActive(false);
                
                foreach (var go in ForRelease)
                    go.SetActive(true);

                foreach (var img in ForRecordingImages)
                    img.color = Utility.exploredAlpha;
                
                Data.StaticData.LevelsData.AlwaysLoadLevelId = -1;

            }
            else if (AppMode == AppMode.Debug)
            {
                foreach (var go in ForRelease)
                    go.SetActive(false);

                foreach (var go in ForRecording)
                    go.SetActive(false);
                
                foreach (var go in ForDebug)
                    go.SetActive(true);

                foreach (var img in ForRecordingImages)
                    img.color = Utility.exploredAlpha;
            }
            else if (AppMode == AppMode.Creative)
            {
                foreach (var go in ForRelease)
                    go.SetActive(false);

                foreach (var go in ForDebug)
                    go.SetActive(false);

                foreach (var go in ForRecording)
                    go.SetActive(true);

                foreach (var img in ForRecordingImages)
                    img.color = Utility.zeroAlpha;
            }
        }
    }

    public enum AppMode
    {
        Debug,
        Release,
        Creative
    }
}