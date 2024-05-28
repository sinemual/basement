using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageFeelingScreen : BaseScreen
{
    [SerializeField] private Image damageFeelImage;
    
    protected override void ManualStart()
    {
        GameUi.EventBus.PlayerDamage.PlayerDamageFeelingEvent += PlayerGetDamage;
    }

    private void PlayerGetDamage()
    {
        damageFeelImage.DORewind();
        damageFeelImage.DOFade(1.0f, 0.2f).SetLoops(2, LoopType.Yoyo);
    }
}