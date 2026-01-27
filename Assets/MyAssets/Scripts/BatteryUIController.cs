using System;
using UnityEngine;
using UnityEngine.UI;

public class BatteryUIController : MonoBehaviour
{
    [Header("UI References")]
    public Image batteryFillImage;
    
    private FlashlightController _flashlight;

    private void Update()
    {
        if (_flashlight == null)
        {
            _flashlight = FlashlightController.instance;
            return;
        }

        batteryFillImage.fillAmount = _flashlight.GetBatteryPercent();
    }
}
