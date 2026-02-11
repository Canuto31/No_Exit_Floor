using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    [Header("Vision Settings")]
    public float viewDistance = 8f;
    [Range(0, 180)]
    public float viewAngle = 60f;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float stopDistance  = 1.5f;
    public float rotationSpeed = 5f;
    
    private Rigidbody _rb;
    private bool _playerDetected;
    
    private PlayerBreathingController _breathing;
    private bool _wasPlayerDetected;
    private PlayerFearShake _fearShake;

    private float _originalMoveSpeed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _breathing = PlayerBreathingController.instance;
        _fearShake = player.GetComponentInChildren<PlayerFearShake>();
        
        _originalMoveSpeed = moveSpeed;
    }
    
    private void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        
        _playerDetected = CanSeePlayer();
        
        if (_playerDetected && !_wasPlayerDetected)
        {
            PlayerBreathingController.instance.PlayStressedBreathing();
            _fearShake?.SetFearShake(true);
        }
        else if (!_playerDetected && _wasPlayerDetected)
        {
            PlayerBreathingController.instance.PlayCalmBreathing();
            _fearShake?.SetFearShake(false);
        }

        _wasPlayerDetected = _playerDetected;

        if (_playerDetected && distance > stopDistance)
        {
            ChasePlayer();
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > viewDistance) return false;
        
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer > viewAngle / 2f) return false;

        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, directionToPlayer);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, viewDistance))
        {
            if (!hit.transform.CompareTag("Player"))
                return false;
        }

        return true;
    }
    
    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        Vector3 movePosition = transform.position + direction * moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(movePosition);

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        // Distancia
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        // Cono de visión
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewDistance);

        // Línea frontal
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewDistance);
    }

    public void SetVulnerableMovement(bool isVulnerable)
    {
        moveSpeed = isVulnerable ? _originalMoveSpeed * 0.5F : _originalMoveSpeed;
    }
}
