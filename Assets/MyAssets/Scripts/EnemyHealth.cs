using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    private float _currentHealth;

    private EnemyHitReaction _hitReaction;

    private void Awake()
    {
        _currentHealth = maxHealth;
        _hitReaction = GetComponent<EnemyHitReaction>();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        Debug.Log($"💥 Enemy hit! HP: {_currentHealth}");

        if (_hitReaction != null)
            OnHit();

        if (_currentHealth <= 0)
            Die();
    }

    private void OnHit()
    {
        Debug.Log("😵 Enemy stagger");
        _hitReaction.React();

    }

    private void Die()
    {
        Debug.Log("☠ Enemy dead");
        gameObject.SetActive(false);
    }
}
