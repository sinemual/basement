using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ToonMaker : MonoBehaviour
{
    public List<Material> materials;
    //public List<MaterialWithOriginalColor> materialsWithOriginalColor;

    [Header("Manual Settings")]
    //public bool isChangeColor;
    //public bool isBackToOriginalColor;

    public float lightContribution;
    public float unityShadowPower;
    public float fullSaturationChange;
    public float fullValueChange;

    public float selfShadingSize;
    public float extraSelfShadingSize;

    public float saturationChange;
    public float valueChange;

    public float extraSaturationChange;
    public float extraValueChange;

    public float gradientSaturationChange;
    public float gradientValueChange;
    public float gradientSize;

    float m_Hue = 0;
    float m_Saturation = 0;
    float m_Value = 0;

    [Button]
    public void Tooning()
    {
        int _counter = -1;

        foreach (var item in materials)
        {
            _counter++;

            //if (isChangeColor)
            //    originalColor = item.color;

            //if (isBackToOriginalColor)
            //item.color = originalColor;

            if (!item.HasProperty("_SelfShadingSize"))
            {
                //originalColor = item.color;
                item.shader = Shader.Find("FlatKit/Stylized Surface");
            }

            Color.RGBToHSV(item.color, out m_Hue, out m_Saturation, out m_Value);

            item.SetColor("_BaseColor", Color.HSVToRGB(m_Hue, m_Saturation + fullSaturationChange, m_Value + fullValueChange));
            item.SetColor("_ColorDim", Color.HSVToRGB(m_Hue, m_Saturation + saturationChange + fullSaturationChange, m_Value + valueChange + fullValueChange));

            item.SetFloat("_SelfShadingSize", selfShadingSize);
            item.SetFloat("_ShadowEdgeSize", 0f);
            item.SetFloat("_Flatness", 1.0f);

            item.SetInt("_CelExtraEnabled", 1);
            item.SetColor("_ColorDimExtra", Color.HSVToRGB(m_Hue, m_Saturation + extraSaturationChange + fullSaturationChange, m_Value + extraValueChange + fullValueChange));
            item.SetFloat("_SelfShadingSizeExtra", extraSelfShadingSize);
            item.SetFloat("_ShadowEdgeSizeExtra", 0f);
            item.SetFloat("_FlatnessExtra", 1.0f);

            item.SetInt("_GradientEnabled", 1);
            item.SetColor("_ColorGradient", Color.HSVToRGB(m_Hue, m_Saturation + gradientSaturationChange + fullSaturationChange, m_Value + gradientValueChange + fullValueChange));
            item.SetFloat("_GradientSize", gradientSize);

            item.SetFloat("_LightContribution", lightContribution);

            item.SetFloat("_UnityShadowMode", 1.0f);
            item.SetFloat("_UnityShadowPower", unityShadowPower);
            item.SetColor("_UnityShadowColor", Color.HSVToRGB(m_Hue, m_Saturation + extraSaturationChange + fullSaturationChange, m_Value + extraValueChange + fullValueChange));
        }
    }
}


public class MaterialWithOriginalColor
{
    public Material material;
    public Color originalColor;
}