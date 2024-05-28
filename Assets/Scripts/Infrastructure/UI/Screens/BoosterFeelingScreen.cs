using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BoosterFeelingScreen : BaseScreen
{
    [SerializeField] private Image boosterFeelImage;
    
    protected override void ManualStart()
    {
        OnShowScreen.AddListener(StartBoosterFeeling);
        OnHideScreen.AddListener(StopBoosterFeeling);
    }

    private void StartBoosterFeeling()
    {
        boosterFeelImage.DORewind();
        boosterFeelImage.DOFade(0.5f, 1.0f).SetLoops(-1, LoopType.Yoyo);
    }
    
    private void StopBoosterFeeling()
    {
        boosterFeelImage.DORewind();
        boosterFeelImage.DOKill();
    }
}