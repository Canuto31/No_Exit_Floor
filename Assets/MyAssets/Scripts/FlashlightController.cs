using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlashlightController : MonoBehaviour
{
    public static FlashlightController instance;

    [Header("References")] 
    public Light flashlightLight;
    public GameObject flashlightModel;

    [Header("Batter Settings")] 
    public float maxBatteryTime = 30f;
    public float lowBatteryThreshold = 5f;

    [Header("Flicker Settings")] 
    public float flickerSpeed = 0.1f;
    public float flickerIntensity = 0.6f;

    private float _currentBatteryTime;
    private bool _isOn = false;

    private float _baseIntensity;
    private float _flickerTimer;

    private void Awake()
    {
        instance = this;
        
        _currentBatteryTime = maxBatteryTime;
        _baseIntensity = flashlightLight.intensity;
        
        SetFlashlight(false);
    }

    private void Update()
    {
        if (!_isOn) return;

        DrainBattery();
        HandleFlicker();
    }

    public void Toggle()
    {
        if (_currentBatteryTime <= 0) return;
        
        SetFlashlight(!_isOn);
    }

    private void SetFlashlight(bool state)
    {
        _isOn = state;

        flashlightLight.enabled = state;

        if (!state)
            flashlightLight.intensity = _baseIntensity;
    }

    private void DrainBattery()
    {
        _currentBatteryTime -= Time.deltaTime;

        if (_currentBatteryTime <= 0f)
        {
            _currentBatteryTime = 0f;
            SetFlashlight(false);
        }
    }

    private void HandleFlicker()
    {
        if (_currentBatteryTime > lowBatteryThreshold) return;

        _flickerTimer -= Time.deltaTime;

        if (_flickerTimer <= 0f)
        {
            float randomIntensity = Random.Range(_baseIntensity * flickerIntensity, _baseIntensity);
            
            flashlightLight.intensity = randomIntensity;
            _flickerTimer = flickerSpeed;
        }
    }

    public bool IsOn()
    {
        return _isOn;
    }

    public float GetBatteryPercent()
    {
        return _currentBatteryTime / maxBatteryTime;
    }

    public void RechargeBattery()
    {
        _currentBatteryTime = maxBatteryTime;
        
        if (_isOn)
            flashlightLight.intensity = _baseIntensity;
        
        Debug.Log("🔋 Flashlight battery recharged");
    }
}