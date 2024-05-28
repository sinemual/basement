using Client;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;

public class CameraControlScreen : BaseScreen
{
    [SerializeField] private ActionButton turnRightButton;
    [SerializeField] private ActionButton turnLeftButton;

    protected override void ManualStart()
    {
        turnRightButton.OnClickEvent.AddListener(() =>
            {
                GameUi.EventBus.CameraControlScreen.OnTurnRightButtonTap();
            }
        );
        
        turnLeftButton.OnClickEvent.AddListener(() =>
            {
                GameUi.EventBus.CameraControlScreen.OnTurnLeftButtonTap();
            }
        );
    }
}