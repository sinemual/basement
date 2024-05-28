using System;

public class CameraControlScreenEvents
{
    public event Action TurnRightButtonTap;
    public event Action TurnLeftButtonTap;
    public void OnTurnRightButtonTap() => TurnRightButtonTap?.Invoke();
    public void OnTurnLeftButtonTap() => TurnLeftButtonTap?.Invoke();
}