using System;
using UnityEngine;

public class InteractableGlow : MonoBehaviour
{
    [Header("Glow Settings")]
    public float pulseSpeed = 2f;
    public float minIntensity = 0.2f;
    public float maxIntensity = 1.2f;
    
    private Material _material;
    private Color _baseEmissionColor;
    private bool _isActive;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        
        _material.EnableKeyword("_EMISSION");
        
        _baseEmissionColor = _material.GetColor("_EmissionColor");

        SetGlow(true);
    }

    private void Update()
    {
        if (!_isActive) return;

        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, pulse);

        _material.SetColor("_EmissionColor", _baseEmissionColor * intensity);
    }

    public void SetGlow(bool state)
    {
        _isActive = state;
        
        if (!state)
            _material.SetColor("_EmissionColor", _baseEmissionColor * minIntensity);
    }
}
