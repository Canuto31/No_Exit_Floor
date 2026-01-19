using System;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment instance;

    public GameObject flashlightObejct;
    public GameObject pistolObject;

    private void Awake()
    {
        instance = this;
    }

    public void Equip(EquippableType type)
    {
        switch (type)
        {
            case EquippableType.Flashlight:
                flashlightObejct.SetActive(true);
                break;
            case EquippableType.Pistol:
                pistolObject.SetActive(true);
                break;
        }
    }
}
