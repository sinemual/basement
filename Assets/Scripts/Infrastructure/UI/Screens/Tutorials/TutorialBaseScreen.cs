using Client.Data;
using Leopotam.Ecs;
using UnityEngine;

public class TutorialBaseScreen : BaseScreen
{
    //[SerializeField] private StaticData.Enums.Tutorials tutorial;
    //[SerializeField] private ActionButton actionButton;
    //[SerializeField] private bool isOneTapTutorial;
    
    protected override void ManualStart()
    {
        /*if (isOneTapTutorial)
        {
            actionButton.OnClickEvent.AddListener(() =>
                EcsWorld.NewEntity().Get<CompleteTutorialRequest>().Tutorial = tutorial);
            actionButton.OnClickEvent.AddListener(() => SetShowState(false));
        }*/
    }
}