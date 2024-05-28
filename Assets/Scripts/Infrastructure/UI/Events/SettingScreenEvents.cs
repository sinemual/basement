using System;

public class SettingScreenEvents
{
    public event Action VibrationTriggerButtonTap;
    public event Action SoundTriggerButtonTap;
    public void OnVibrationTriggerButtonTap() => VibrationTriggerButtonTap?.Invoke();
    public void OnSoundTriggerButtonTap() => SoundTriggerButtonTap?.Invoke();
}