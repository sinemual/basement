using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using ShadowQuality = UnityEngine.ShadowQuality;

public class DynamicResolution : MonoBehaviour
{
    [SerializeField] float maxDPI = 0.95f;
    [SerializeField] float minDPI = 0.55f;
    [SerializeField] float dampen = 0.02f;


    [SerializeField] float maxFps = 75;
    [SerializeField] float minFps = 55;

    [SerializeField] float renderScale = 1f;
    [SerializeField] float refreshResolutionTime = 1f;

    private float timer = 0;
    private float deltaTime;

    private void Awake()
    {
        QualitySettings.SetQualityLevel(0, true);
        //StartCoroutine(ShadowFPSCheck());
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            ResolutionUpdate();
            timer = refreshResolutionTime;
        }
    }

    private void ResolutionUpdate()
    {
        float fps = GetFps();
    }
    private float GetFps()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        return fps;
    }

    private IEnumerator ShadowFPSCheck()
    {
        yield return new WaitForSeconds(5.5f);
        if (GetFps() > minFps)
            QualitySettings.SetQualityLevel(1,true);
        else
            QualitySettings.SetQualityLevel(0, true);
    }
}