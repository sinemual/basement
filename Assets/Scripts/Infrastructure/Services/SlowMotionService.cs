using System;
using System.Collections;
using Client;
using UnityEngine;

public class SlowMotionService
{
    private ICoroutineRunner _coroutineRunner;

    public SlowMotionService(ICoroutineRunner coroutineRunner)
    {
        _coroutineRunner = coroutineRunner;
    }

    /*public void StartSlowMotion(float startValue, float endValue, float time)
    {
        Time.timeScale = startValue;
        DOTween.To(() => startValue, x => Time.timeScale = x, endValue, time);
    }*/

    public void StartSlowMotion(float startValue, float endValue, float time)
    {
        _coroutineRunner.StartCoroutine(SlowMo(startValue, endValue, time));
    }
    
    private IEnumerator SlowMo(float startValue, float endValue, float time)
    {
        Time.timeScale = startValue;
        float currentn = 0.0f;
        var t = Time.unscaledTime;
        while (currentn <= 1.0f)
        {
            yield return null;
            Time.timeScale = Mathf.Lerp(startValue, endValue, currentn);
            currentn += Time.unscaledDeltaTime / time;
        }
    }
}