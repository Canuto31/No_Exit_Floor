using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    private float _currentHealth;

    private EnemyHitReaction _hitReaction;

    [Header("State")]
    public EnemyState currentState = EnemyState.Fog;
    
    [Header("Impact FX")]
    public GameObject bloodImpactPrefab;
    public GameObject fogImpactPrefab;

    private void Awake()
    {
        _currentHealth = maxHealth;
        _hitReaction = GetComponent<EnemyHitReaction>();
    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Quaternion hitRotation = Quaternion.LookRotation(hitNormal);
        hitPoint += hitNormal * 0.01f;

        if (currentState == EnemyState.Fog)
        {
            if (fogImpactPrefab)
            {
                GameObject fogTemp = Instantiate(fogImpactPrefab, hitPoint, hitRotation);
                fogTemp.transform.parent = transform;
            }
            
            
            return;
        }
        
        if (bloodImpactPrefab)
            Instantiate(bloodImpactPrefab, hitPoint, hitRotation);
        
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
