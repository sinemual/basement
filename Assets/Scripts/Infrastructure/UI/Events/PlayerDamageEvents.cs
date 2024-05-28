using System;

public class PlayerDamageEvents
{
    public event Action<float, float> PlayerDamageEvent;
    public event Action<float, float> InitPlayerHealthEvent;
    public event Action PlayerDamageFeelingEvent;
    public void OnPlayerDamageEvent(float fullHealth, float currentHealth) => PlayerDamageEvent?.Invoke(fullHealth, currentHealth);
    public void OnInitPlayerHealthEvent(float fullHealth, float currentHealth) => InitPlayerHealthEvent?.Invoke(fullHealth, currentHealth);
    public void OnPlayerDamageFeelingEvent() => PlayerDamageFeelingEvent?.Invoke();
}