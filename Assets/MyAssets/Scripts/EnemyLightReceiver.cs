using System;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyLightReceiver : MonoBehaviour
{
    [Header("Light Detection")]
    public float requiredExposureTime = 3f;
    public float vulnerableDuration = 5f;
    public float maxLightDistance = 10f;

    [Range(0f, 90f)]
    public float lightAngle = 25f;

    private float _currentExposureTime;
    private float _vulnerableTimer;

    private EnemyHealth _enemyHealth;
    private FlashlightController _flashlight;
    private Transform _flashlightTransform;

    private EnemyAI _enemyAI;
    
    [Header("VFX")]
    [SerializeField] private VisualEffect vulnerabilityVFX;
    private readonly string colorParam = "Smoke Color";

    [SerializeField] private Color colorVulnerable;

    private void Awake()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
        _flashlight = FlashlightController.instance;
        _enemyAI = GetComponent<EnemyAI>();
    }

    private void Update()
    {
        if (_enemyHealth == null) return;
        
        if (_flashlight == null)
        {
            _flashlight = FlashlightController.instance;
            if (_flashlight == null)
                return;
        }

        if (_enemyHealth.currentState == EnemyState.Fog)
        {
            HandleFogState();
        }
        else if (_enemyHealth.currentState == EnemyState.Vulnerable)
        {
            HandleVulnerableState();
        }
    }

    private void HandleFogState()
    {
        if (!IsBeingIluminated())
        {
            _currentExposureTime = 0f;
            return;
        }
        
        _currentExposureTime += Time.deltaTime;

        if (_currentExposureTime >= requiredExposureTime)
        {
            BecomeVulnerable();
        }
    }

    private void HandleVulnerableState()
    {
        _vulnerableTimer -= Time.deltaTime;

        if (_vulnerableTimer <= 0)
        {
            ReturnToFog();
        }
    }

    private bool IsBeingIluminated()
    {
        if (!_flashlight.IsOn())
            return false;

        Light light = _flashlight.flashlightLight;
        if (light == null)
            return false;
        
        //Vector3 directionToEnemy = (transform.position - light.transform.position);
        Collider enemyCollider = GetComponent<Collider>();
        Vector3 closestPoint = enemyCollider.ClosestPoint(light.transform.position);
        
        Vector3 directionToEnemy = closestPoint - light.transform.position;
        float distance = directionToEnemy.magnitude;

        if (distance > maxLightDistance)
            return false;
        
        directionToEnemy.Normalize();

        float angle = Vector3.Angle(light.transform.forward, directionToEnemy);
        if (angle > lightAngle)
            return false;

        Ray ray = new Ray(light.transform.position, directionToEnemy);
        if (Physics.Raycast(ray, out RaycastHit hit, maxLightDistance))
        {
            if (hit.transform != transform)
                return false;
        }
        
        
        return true;
    }

    private void BecomeVulnerable()
    {
        _enemyHealth.currentState = EnemyState.Vulnerable;
        _vulnerableTimer = vulnerableDuration;
        _currentExposureTime = 0f;
        
        _enemyAI?.SetVulnerableMovement(true);

        if (vulnerabilityVFX == null) return;
        
        vulnerabilityVFX.SetVector4(colorParam, colorVulnerable);
        
        Debug.Log("🔥 Enemy is now VULNERABLE");
    }

    private void ReturnToFog()
    {
        _enemyHealth.currentState = EnemyState.Fog;
        
        _enemyAI?.SetVulnerableMovement(false);
        
        vulnerabilityVFX.SetVector4(colorParam, Color.black);
        
        Debug.Log("🌫 Enemy returned to FOG");
    }
    
    private void OnDrawGizmosSelected()
    {
        if (FlashlightController.instance == null)
            return;

        Light light = FlashlightController.instance.flashlightLight;
        if (light == null)
            return;

        Transform lightTransform = light.transform;

        Gizmos.color = Color.yellow;

        // 📏 Distancia máxima de la linterna
        Gizmos.DrawWireSphere(lightTransform.position, maxLightDistance);

        // 🔦 Cono de luz
        Vector3 forward = lightTransform.forward;

        Vector3 leftBoundary = Quaternion.Euler(0, -lightAngle, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, lightAngle, 0) * forward;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(
            lightTransform.position,
            lightTransform.position + leftBoundary * maxLightDistance
        );

        Gizmos.DrawLine(
            lightTransform.position,
            lightTransform.position + rightBoundary * maxLightDistance
        );

        // ➡️ Línea hacia el enemigo
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            lightTransform.position,
            transform.position
        );
    }
}
