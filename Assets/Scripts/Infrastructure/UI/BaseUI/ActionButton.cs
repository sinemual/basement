using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActionButton : UIElement, IPointerDownHandler
{
    private static readonly int OnClickTriggerName = Animator.StringToHash("OnClick");
    [HideInInspector] public UnityEvent OnClickEvent;
    [HideInInspector] public UnityEvent OnPointerDownEvent;

    private Button _unityButton;

    private void Awake()
    {
        _unityButton = GetComponent<Button>();
        _unityButton.onClick.AddListener(() => OnClickEvent.Invoke());
        OnClickEvent.AddListener(OnClickEventReaction);
    }

    private void OnDestroy()
    {
        _unityButton.onClick.RemoveAllListeners();
        OnClickEvent.RemoveAllListeners();
    }

    private void OnClickEventReaction()
    {
        Animator.SetTrigger(OnClickTriggerName);
    }
    
    public void SetInteractable(bool flag)
    {
        if (!_unityButton)
            _unityButton = GetComponent<Button>();

        _unityButton.interactable = flag;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownEvent.Invoke();
    }
}