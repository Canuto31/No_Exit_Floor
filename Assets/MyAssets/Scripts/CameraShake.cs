using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    
    private Vector3 _originalLocalPos;
    private Coroutine _shakeRoutine;

    private void Awake()
    {
        instance = this;
        _originalLocalPos = transform.localPosition;
    }

    public void Shake(float intensity = 0.05f, float duration = 0.1f)
    {
        if (_shakeRoutine != null)
            StopCoroutine(_shakeRoutine);

        _shakeRoutine = StartCoroutine(ShakeRoutine(intensity, duration));
    }

    private IEnumerator ShakeRoutine(float intensity, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 offset = UnityEngine.Random.insideUnitSphere * intensity;
            transform.localPosition = _originalLocalPos + offset;
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.localPosition = _originalLocalPos;
    }
}
