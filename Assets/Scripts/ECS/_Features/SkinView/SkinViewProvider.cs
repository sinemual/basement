using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public struct SkinViewProvider
{
    [HideIf("IsRandomSkin")]
    public int SkinViewNum;
    public List<GameObject> SkinViews;
    public bool IsRandomSkin;
}