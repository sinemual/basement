using System.Collections.Generic;
using System.Linq;
using Client.Data;
using Client.Data.Core;
using Client.Infrastructure.Services;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class TutorialSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;

        private AnalyticService _analyticService;

        private EcsFilter<TutorialProvider> _tutorialFilter;
        private EcsFilter<StartTutorialRequest> _startFilter;
        private EcsFilter<CompleteTutorialRequest> _completeFilter;

        public void Init()
        {
        }

        public void Run()
        {
            foreach (var idx in _startFilter)
            {
                ref EcsEntity entity = ref _startFilter.GetEntity(idx);
                ref StartTutorialRequest startRequest = ref entity.Get<StartTutorialRequest>();

                _ui.Tutorials[startRequest.TutorialStep].SetShowState(true);

                List<BaseScreen> screens = _ui.Tutorials[startRequest.TutorialStep].transform.GetComponentsInChildren<BaseScreen>(true).ToList();
                screens.RemoveAt(0);
                foreach (var screen in screens)
                    screen.SetShowState(true);

                _data.PlayerData.CurrentTutorialStep = startRequest.TutorialStep;
                Debug.Log($"Start tutorial: {startRequest.TutorialStep}");
                foreach (var tutrProvider in _tutorialFilter)
                {
                    ref var tutr = ref _tutorialFilter.GetEntity(tutrProvider).Get<TutorialProvider>();
                    if (tutr.TutorialStep == startRequest.TutorialStep)
                    {
                        foreach (var go in tutr.TutorialGameObjects)
                            if (go != null)
                                go.SetActive(true);
                    }
                }

                if (_data.StaticData.Tutorials[_data.PlayerData.CurrentTutorialStep].IsBlockingAllRaycastExpectTutorial)
                    _data.RuntimeData.IsBlockingRaycastForTutorial = true;

                if (_data.StaticData.Tutorials[_data.PlayerData.CurrentTutorialStep].Is3DTutorial && 
                    _data.StaticData.Tutorials[_data.PlayerData.CurrentTutorialStep].IsBlockingAllRaycastExpectTutorial)
                {
                    //_data.StaticData.TutorialRender.SetActive(true);
                    _data.StaticData.TutorialMaterial.DOColor(new Color(1.0f, 1.0f, 1.0f), 0.0f).OnComplete(() =>
                    {
                        _data.StaticData.TutorialMaterial.DOColor(new Color(0.3f, 0.3f, 0.3f), 1.0f);
                    });
                }

                _analyticService.LogEventWithParameter("tutorial_start", _data.PlayerData.CurrentTutorialStep.ToString());

                entity.Del<StartTutorialRequest>();
            }

            foreach (var idx in _completeFilter)
            {
                ref EcsEntity entity = ref _completeFilter.GetEntity(idx);
                ref CompleteTutorialRequest completeRequest = ref entity.Get<CompleteTutorialRequest>();

                _ui.Tutorials[completeRequest.TutorialStep].SetShowState(false);
                _data.PlayerData.TutrorialStates[completeRequest.TutorialStep] = true;

                Debug.Log($"Complete tutorial: {completeRequest.TutorialStep}");

                foreach (var tutrProvider in _tutorialFilter)
                {
                    ref var tutr = ref _tutorialFilter.GetEntity(tutrProvider).Get<TutorialProvider>();
                    if (tutr.TutorialStep == completeRequest.TutorialStep)
                    {
                        foreach (var go in tutr.TutorialGameObjects)
                            if (go != null)
                                go.SetActive(false);
                    }
                }

                if (_data.StaticData.Tutorials[_data.PlayerData.CurrentTutorialStep].IsBlockingAllRaycastExpectTutorial)
                    _data.RuntimeData.IsBlockingRaycastForTutorial = false;
                
                if (_data.StaticData.Tutorials[_data.PlayerData.CurrentTutorialStep].Is3DTutorial && 
                    _data.StaticData.Tutorials[_data.PlayerData.CurrentTutorialStep].IsBlockingAllRaycastExpectTutorial)
                {
                    _data.StaticData.TutorialMaterial.DOColor(new Color(1.0f, 1.0f, 1.0f), 0.5f).OnComplete(() =>
                    {
                        //_data.StaticData.TutorialRender.SetActive(false);
                    });
                }

                if (_data.StaticData.Tutorials[_data.PlayerData.CurrentTutorialStep].IsNextStepDependiced)
                    _world.NewEntity().Get<StartTutorialRequest>().TutorialStep = (TutorialStep)((int)_data.PlayerData.CurrentTutorialStep + 1);

                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.UseItems) // mechanics last
                {
                    foreach (var recipe in _data.StaticData.ItemRecipes)
                        _data.PlayerData.PlayerOpenedRecipes.Remove(recipe.Id);
                    _data.PlayerData.IsMechanicsTutorialComplete = true;
                }

                if (_data.PlayerData.CurrentTutorialStep == TutorialStep.EndTutorialAndStartPlay) // meta last
                {
                    _data.PlayerData.IsMetaTutorialComplete = true;
                    _data.RuntimeData.IsBlockingRaycastForTutorial = false;
                }

                _analyticService.LogEventWithParameter("tutorial_complete", _data.PlayerData.CurrentTutorialStep.ToString());
                _world.NewEntity().Get<TutorialCompleteEvent>().Value = _data.PlayerData.CurrentTutorialStep;
                entity.Del<CompleteTutorialRequest>();
            }
        }
    }
}