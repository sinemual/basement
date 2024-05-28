using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class EcsBaseActionButtonView : UIElement
{
    protected const string OnClickTriggerName = "OnClick";
    protected Button button { get; private set; }

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickAction);
        Init();
    }

    private void OnDestroy()
    {
        button?.onClick.RemoveListener(OnClickAction);
    }

    private void OnValidate()
    {
        //var clickAction = GetComponent<Leopotam.Ecs.Ui.Actions.EcsUiClickAction>();
    }

    protected abstract void OnClickAction();

    //protected abstract Action SetWidgetName(string widgetName);
    protected abstract void Init();
}