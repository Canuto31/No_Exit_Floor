using UnityEngine;

public class EquippablePickup : MonoBehaviour, IInteractable
{
    
    public EquippableType equippableType;
    public void Interact()
    {
        PlayerEquipment.instance.Equip(equippableType);
        gameObject.SetActive(false);
    }
}
