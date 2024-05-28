using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class UIElement : MonoBehaviour
{
    private const string IsHideStateName = "Hide";
    private const string IsShowStateName = "Show";
    private static readonly int IsShow = Animator.StringToHash("IsShow");
    protected Animator Animator;

    private bool _isHaveAnimtor;

    private void OnEnable()
    {
        if (transform.TryGetComponent(out Animator animator))
        {
            Animator = animator;
            _isHaveAnimtor = true;
        }
        else
            _isHaveAnimtor = false;
    }

    public virtual void SetShowState(bool isShow)
    {
        if (isShow)
            Show();
        else
            Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        if (!_isHaveAnimtor)
            return;
        Animator.SetBool(IsShow, true);
    }

    private void Hide()
    {
        if (!_isHaveAnimtor)
        {
            gameObject.SetActive(false);
            return;
        }

        Animator.SetBool(IsShow, false);
        if (Animator.gameObject.activeInHierarchy)
            StartCoroutine(DisableDelay());
    }

    private IEnumerator DisableDelay()
    {
        var closedStateReached = false;
        var wantToClose = true;
        while (!closedStateReached && wantToClose)
        {
            if (!Animator.IsInTransition(0))
                closedStateReached = Animator.GetCurrentAnimatorStateInfo(0).IsName(IsHideStateName);

            wantToClose = !Animator.GetBool(IsShow);

            yield return new WaitForEndOfFrame();
        }

        if (wantToClose)
            Animator.gameObject.SetActive(false);
    }
}