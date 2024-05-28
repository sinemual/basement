using System;

public class OpenGameProgressScreenEvents
{
    public event Action OpenGameProgressScreenButtonTap;
    public void OnGameProgressScreenButtonTap() => OpenGameProgressScreenButtonTap?.Invoke();
}