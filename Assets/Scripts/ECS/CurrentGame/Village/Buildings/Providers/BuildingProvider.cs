using System;
using System.Collections.Generic;
using Client;
using Client.Data;
using Client.Data.Equip;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct BuildingProvider
{
    public BuildingType Type;
    
    public GameObject BuidlingPlace;
    public List<GameObject> BuidlingByLevel;

    public GameObject TimerPanel;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI LevelText;
    public Image IncomeProgressBarImage;

    public GameObject BuildPanel;
    public ActionButton OpenPanelButton;

    public GameObject IncomePanel;
    public ResourcePanel IncomeResourcePanel;
    public ActionButton GetIncomeResourcesButton;
    
    public ParticleSystem BuildDustVFX;
}