using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusScreen : BaseScreen
{
    [SerializeField] private GameObject healthPanel;
    [SerializeField] private List<GameObject> healthImages;
    [SerializeField] private List<GameObject> damageImages;
    [SerializeField] private List<GameObject> emptyHealthImages;
    
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Image damageBarImage;
    
    protected override void ManualStart()
    {
        GameUi.EventBus.PlayerDamage.InitPlayerHealthEvent += UpdatePlayerHealth;
        GameUi.EventBus.PlayerDamage.PlayerDamageEvent += UpdatePlayerHealth;
        GameUi.EventBus.PlayerDamage.PlayerDamageFeelingEvent += PlayerGetDamage;
    }

    private void UpdatePlayerHealth(float fullHealth, float currentHealth)
    {
        for (int i = 0; i < healthImages.Count; i++)
        {
            healthImages[i].SetActive(false);
            damageImages[i].SetActive(false);
            emptyHealthImages[i].SetActive(false);
        }

        for (int i = 0; i < (int)fullHealth; i++)
        {
            healthImages[i].SetActive(true);
            damageImages[i].SetActive(true);
            emptyHealthImages[i].SetActive(true);
        }

        healthBarImage.DOFillAmount(currentHealth / fullHealth, 0.1f).OnComplete(() =>
        {
            damageBarImage.DOFillAmount(currentHealth / fullHealth, 0.1f);
        });
    }

    private void PlayerGetDamage()
    {
        healthPanel.gameObject.transform.DORewind(); 
        healthPanel.gameObject.transform.DOPunchScale(Vector3.one * 0.1f, 0.15f, 2, 0.5f);
    }
}