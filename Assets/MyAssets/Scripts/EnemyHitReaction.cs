using System;
using UnityEngine;

public class EnemyHitReaction : MonoBehaviour
{
    [Header("Knockback")]
    public float knockbackForce = 4f;
    public float stunTime = 0.15f;

    [Header("FX")]
    public AudioSource hitSound;

    private Rigidbody _rb;
    private bool _isStunned;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void React()
    {
        if (_isStunned) return;
        
        Vector3 direction = (transform.position - Camera.main.transform.position).normalized;
        direction.y = 0f;
        
        _rb.AddForce(direction * knockbackForce, ForceMode.Impulse);

        if (hitSound != null)
            hitSound.Play();

        StartCoroutine(StunCoroutine());
    }

    private System.Collections.IEnumerator StunCoroutine()
    {
        _isStunned = true;
        yield return new WaitForSeconds(stunTime);
        _isStunned = false;
    }
}
