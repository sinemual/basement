using Client;
using DG.Tweening;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public TextMeshProUGUI loadingProcessText;

    public void UpdateProgressText(float progress)
    {
        loadingProcessText.DOText($"{(int)progress}", 0.1f);
    }
}