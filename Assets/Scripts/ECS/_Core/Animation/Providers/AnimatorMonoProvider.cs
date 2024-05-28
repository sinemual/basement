using UnityEngine;

public class AnimatorMonoProvider : MonoProvider<AnimatorProvider>
{
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        if (Value.Value == null)
            Value = new AnimatorProvider { Value = animator };
    }
}