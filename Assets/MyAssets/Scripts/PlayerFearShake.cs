using System;
using UnityEngine;

public class PlayerFearShake : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeIntensity = 0.03f;
    public float shakeSpeeed = 12f;
    
    private Vector3 _initialLocalPos;
    private bool _isShaking;

    private void Awake()
    {
        _initialLocalPos = transform.localPosition;
    }

    private void Update()
    {
        if (!_isShaking)
        {
            transform.localPosition = _initialLocalPos;
            return;
        }

        float x = (Mathf.PerlinNoise(Time.time * shakeSpeeed, 0f) - 0.5f) * shakeIntensity;
        float y = (Mathf.PerlinNoise(0f, Time.time * shakeSpeeed) - 0.5f) * shakeIntensity;
        
        transform.localPosition = _initialLocalPos + new Vector3(x, y, 0f);
    }

    public void SetFearShake(bool state)
    {
        _isShaking = state;

        if (!state)
            transform.localPosition = _initialLocalPos;
    }
}
