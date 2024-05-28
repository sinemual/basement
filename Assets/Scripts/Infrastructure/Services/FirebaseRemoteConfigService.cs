using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client;
#if remote_congig_firebase
using Firebase.Extensions;
using Firebase.RemoteConfig;
#endif
using UnityEngine;

public class FirebaseRemoteConfigService
{
    public bool IsConected { get; private set; }

    private bool _isFetched;
    public bool IsFetched => _isFetched;

    public FirebaseRemoteConfigService()
    {
        IsConected = false;
        _isFetched = false;
#if remote_congig_firebase
            ConnectFirebase();
#endif
    }
#if remote_congig_firebase
        public void ReFetch()
        {
            FetchDataAsync();
        }

        private void ConnectFirebase()
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {

                    FetchDataAsync();
                    Debug.Log("Firebase.DependencyStatus.Available - Connected");
                    IsConected = true;
                }
                else
                {
                    Debug.LogError(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                }
            });
        }
        
        public Task FetchDataAsync()
        {
            Debug.Log("Fetching data...");
            Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

        private void FetchComplete(Task fetchTask)
        {
            if (!fetchTask.IsCompleted)
            {
                Debug.LogError("Retrieval hasn't finished.");
                return;
            }

            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            var info = remoteConfig.Info;
            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
                return;
            }

            // Fetch successful. Parameter values must be activated to use.
            remoteConfig.ActivateAsync()
                .ContinueWithOnMainThread(
                    task =>
                    {
                        Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
                        _isFetched = true;
                    });
            
        }
#endif
}