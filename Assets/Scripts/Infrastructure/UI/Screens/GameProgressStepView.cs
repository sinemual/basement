using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class GameProgressStepView : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI descriptionText;
    public Image backgroundImage;
    public Image rewardImage;
    public ActionButton getRewardButton;
}