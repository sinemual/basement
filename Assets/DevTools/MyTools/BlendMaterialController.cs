using UnityEngine;

public class BlendMaterialController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private int _materialIndex;

    [SerializeField] private AnimationCurve _colorCurve;
    [SerializeField] private Gradient _colorGradient;

    [SerializeField] private AnimationCurve _emmisionCurve;
    [SerializeField] private Gradient _emmisionGradient;

    public void UpdateColor(float value)
    {
        if (_meshRenderer)
            _meshRenderer.materials[_materialIndex].color = _colorGradient.Evaluate(_colorCurve.Evaluate(value)); // old - Color.Lerp(_needed, _origin, value);
        else if (_skinnedMeshRenderer)
            _skinnedMeshRenderer.materials[_materialIndex].color = _colorGradient.Evaluate(_colorCurve.Evaluate(value));
    }

    public void UpdateEmmision(float value, float intensity)
    {
        if (_meshRenderer)
        {
            _meshRenderer.materials[_materialIndex].EnableKeyword("_EMISSION");
            _meshRenderer.materials[_materialIndex].
                SetColor("_EmissionColor", _emmisionGradient.Evaluate(0) * Mathf.LinearToGammaSpace(_emmisionCurve.Evaluate(intensity)));
        }
        else if (_skinnedMeshRenderer)
        {
            _skinnedMeshRenderer.materials[_materialIndex].EnableKeyword("_EMISSION");
            _skinnedMeshRenderer.materials[_materialIndex].
                SetColor("_EmissionColor", _emmisionGradient.Evaluate(0) * Mathf.LinearToGammaSpace(_emmisionCurve.Evaluate(intensity)));
        }
    }
}
