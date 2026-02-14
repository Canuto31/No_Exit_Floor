using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float timeToDie = 10f;
    [SerializeField] private DeathManager deathManager;

    private void Start()
    {
        Invoke(nameof(Die), timeToDie);
    }

    void Die()
    {
        if (deathManager != null)
        {
            deathManager.ShowDeathScreen();
        }
        else
        {
            Debug.LogError("DeathManager no asignado en PlayerHealth");
        }
    }
}
