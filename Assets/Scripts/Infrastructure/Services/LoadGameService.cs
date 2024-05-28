using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Data.Core;
using UnityEngine;

public class LoadGameService
{
    private SharedData _data;
    private GameUI _ui;

    public LoadGameService(SharedData data, GameUI ui)
    {
        _data = data;
        _ui = ui;
    }

    public async Task StartGameLoad()
    {
        //Levels
        if (_data.StaticData.LevelsData.AlwaysLoadLevelId != -1)
            _data.PlayerData.CurrentLevelIndex = _data.StaticData.LevelsData.AlwaysLoadLevelId;
        /*
        var levelsHandle =
            Addressables.LoadAssetsAsync<GameObject>(_gameData.StaticData.LevelsData.Levels[_gameData.PlayerData.CurrentLevelPackIndex], null);
            */

        //Other
        //List<AsyncOperationHandle<GameObject>> allHandles = new List<AsyncOperationHandle<GameObject>>();

        //var xHandle = Addressables.LoadAssetAsync<GameObject>(_gameData.StaticData.PrefabData.x);

        //allHandels.Add(xHandle);

        //await OnStartUpdateLoadingScreenProgress(allHandles);

       //Object.Instantiate(xHandle);
       
        HideLoadingScreen();
    }

    /*private async Task OnStartUpdateLoadingScreenProgress(List<AsyncOperationHandle<GameObject>> handles)
    {
        ShowLoadingScreen();
        //float fakeLoadingTime = 3.12f;
#if UNITY_EDITOR
        //fakeLoadingTime = 0.0f;
#endif
        //int loadNum = 0;
        /*DOTween.To(() => loadNum, x => loadNum = x, 100, fakeLoadingTime)
            .OnUpdate(() => _ui.LoadingScreen.loadingProcessText.text = $"{loadNum:D}");#1#
        
        float progress = 0.0f;
        while (progress < 1.00f)
        {
            progress = handles.Sum(x => x.PercentComplete) / handles.Count;
            await Task.Yield();
        }
    }*/


    private void ShowLoadingScreen()
    {
        //_ui.LoadingScreen.gameObject.SetActive(true);
        //_ui.LoadingScreen.loadingProcessText.text = "0";
    }

    private void HideLoadingScreen()
    {
        //_ui.LoadingScreen.gameObject.SetActive(false);
    }
}