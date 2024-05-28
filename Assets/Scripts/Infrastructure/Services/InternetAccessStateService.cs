using UnityEngine;
using UnityEngine.Events;

public class InternetAccessStateService
{
    private bool isDebugInternet;
    [HideInInspector] public UnityEvent OnInternetAppeared;
    [HideInInspector] public UnityEvent OnInternetDropped;
    private bool isHaveInternet;
    private TimeManagerService _timeManagerService;
    private bool isHaveInternetSavedState;

    public InternetAccessStateService()
    {
        CheckInternetAccess();
    }
    
    private void CheckInternetAccess()
    {
        isHaveInternet = Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
                         Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        if (isHaveInternetSavedState != !isHaveInternet)
        {
            if (isHaveInternet)
                OnInternetAppeared.Invoke();
            else
                OnInternetDropped.Invoke();
        }

        isHaveInternetSavedState = !isHaveInternet;
    }

    public bool IsHaveNetTimeAndInternet()
    {
        return  isHaveInternet;
    }
}