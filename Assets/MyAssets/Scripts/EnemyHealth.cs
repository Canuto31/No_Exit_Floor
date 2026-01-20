using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        Debug.Log($"💥 Enemy hit! HP: {_currentHealth}");

        OnHit();

        if (_currentHealth <= 0)
            Die();
    }

    private void OnHit()
    {
        Debug.Log("😵 Enemy stagger");
    }

    private void Die()
    {
        Debug.Log("☠ Enemy dead");
        gameObject.SetActive(false);
    }
}
