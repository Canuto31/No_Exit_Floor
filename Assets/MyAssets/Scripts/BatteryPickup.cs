using UnityEngine;

public class BatteryPickup : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (FlashlightController.instance != null)
        {
            FlashlightController.instance.RechargeBattery();
        }
        Debug.Log("🔋 Battery picked up");
        
        gameObject.SetActive(false);
        
    }
}
