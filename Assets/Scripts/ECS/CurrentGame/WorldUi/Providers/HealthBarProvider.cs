using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct HealthBarProvider
{
    public GameObject MainPanel;
    public List<GameObject> EmptyHealthImages;
    public List<GameObject> HealthImages;
    public List<GameObject> DamageImages;
    
    public Image HealthBarImage;
    public Image DamageBarImage;
}