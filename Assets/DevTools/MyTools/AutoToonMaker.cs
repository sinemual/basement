using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AutoToonMaker : MonoBehaviour
{
    [SerializeField] private List<Material> tooningMaterials;

    private float firstSaturationChange = -0.05f;
    private float firstValueChange = 0.1f;

    private float secondSaturationChange = 0.1f;
    private float secondValueChange = -0.2f;

    private float lightContribution = 0.2f;
    private float unityShadowPower = 0.3f;

    private float m_Hue;
    private float m_Saturation;
    private float m_Value;

    [Button]
    public void AutoTooning()
    {
        var _counter = -1;
        for (var i = 0; i < tooningMaterials.Count; i++)
        {
            _counter++;
            if (!tooningMaterials[_counter].HasProperty("_SelfShadingSize"))
                tooningMaterials[_counter].shader = Shader.Find("FlatKit/Stylized Surface");

            Color.RGBToHSV(tooningMaterials[_counter].color, out m_Hue, out m_Saturation, out m_Value);

            tooningMaterials[_counter].SetColor("_BaseColor",
                Color.HSVToRGB(m_Hue, m_Saturation + firstSaturationChange, m_Value + firstValueChange));
            tooningMaterials[_counter].SetColor("_ColorDim",
                Color.HSVToRGB(m_Hue, m_Saturation + secondSaturationChange, m_Value + secondValueChange));

            tooningMaterials[_counter].SetFloat("_SelfShadingSize", 0.7f);
            tooningMaterials[_counter].SetFloat("_ShadowEdgeSize", 0f);
            tooningMaterials[_counter].SetFloat("_Flatness", 1.0f);

            tooningMaterials[_counter].SetFloat("_LightContribution", lightContribution);

            tooningMaterials[_counter].SetFloat("_UnityShadowMode", 1.0f);
            tooningMaterials[_counter].SetFloat("_UnityShadowPower", unityShadowPower);
            tooningMaterials[_counter].SetColor("_UnityShadowColor",
                Color.HSVToRGB(m_Hue, m_Saturation + secondSaturationChange, m_Value + secondValueChange));
        }
    }
}