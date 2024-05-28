using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseScreen : UIElement
{
    [HideInInspector] public UnityEvent OnHideScreen;
    [HideInInspector] public UnityEvent OnShowScreen;

    [HideInInspector] protected SharedData SharedData;
    [HideInInspector] protected GameUI GameUi;


    protected bool screenIsShow;

    public bool ScreenIsShow => screenIsShow;

    public void Inject(SharedData sharedData, GameUI gameUi)
    {
        this.SharedData = sharedData;
        this.GameUi = gameUi;
    }

    public void Init()
    {
        GameUi.TimeManagerService.OnSecondUpdate += ManualUpdate;
        screenIsShow = false;
        ManualStart();
    }

    protected abstract void ManualStart();
    protected virtual void ManualUpdate() { }
    
    public override void SetShowState(bool isShow) // [System.Runtime.CompilerServices.CallerMemberName] string memberName = "" - WHO
    {
        base.SetShowState(isShow);
        if (isShow)
        {
            OnShowScreen?.Invoke();
            screenIsShow = true;
        }
        else
        {
            OnHideScreen?.Invoke();
            screenIsShow = false;
        }
    }
}