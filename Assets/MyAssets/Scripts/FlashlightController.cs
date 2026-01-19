using System;
using Unity.VisualScripting;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public static FlashlightController instance;
    
    [Header("References")]
    public Light flashlightLight;
    public GameObject flashlightModel;

    private bool _isOn = false;

    private void Awake()
    {
        instance = this;
        SetFlashlight(false);
    }

    public void Toggle()
    {
        SetFlashlight(!_isOn);
    }

    private void SetFlashlight(bool state)
    {
        _isOn = state;
        
        flashlightLight.enabled = state;
        
        /*if (flashlightLight != null)
            flashlightModel.SetActive(state);*/
    }

    public bool IsOn()
    {
        return _isOn;
    }
}
