using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerBreathingController : MonoBehaviour
{
    public static PlayerBreathingController instance;

    [Header("Audio")]
    public AudioSource breathingAudio;

    [Header("Calm Settings")]
    public float calmPitch = 0.9f;
    public float calmVolume = 0.2f;

    [Header("Stressed Settings")]
    public float stressedMinPitch = 1.2f;
    public float stressedMaxPitch = 1.8f;
    public float stressedVolume = 0.6f;
    public float pitchChangeInterval = 0.5f;

    private Coroutine _pitchRoutine;
    private bool _isStressed;

    private void Awake()
    {
        instance = this;
        breathingAudio.loop = true;
        breathingAudio.playOnAwake = false;
    }
    
    private void Start()
    {
        PlayCalmBreathing();
    }

    public void PlayCalmBreathing()
    {
        _isStressed = false;
        StopPitchRoutine();
        
        breathingAudio.pitch = calmPitch;
        breathingAudio.volume = calmVolume;
        
        if (!breathingAudio.isPlaying)
            breathingAudio.Play();
    }

    public void PlayStressedBreathing()
    {
        if (_isStressed) return;
        _isStressed = true;
        
        breathingAudio.volume = Mathf.Lerp(
            breathingAudio.volume,
            stressedVolume,
            Time.deltaTime * 3f
        );

        StopPitchRoutine();
        _pitchRoutine = StartCoroutine(StressedPitchRoutine());
        
        if (!breathingAudio.isPlaying)
            breathingAudio.Play();
    }

    private IEnumerator StressedPitchRoutine()
    {
        while (true)
        {
            breathingAudio.pitch = Random.Range(stressedMinPitch, stressedMaxPitch);
            yield return new WaitForSeconds(pitchChangeInterval);
        }
    }

    private void StopPitchRoutine()
    {
        if (_pitchRoutine != null)
        {
            StopCoroutine(_pitchRoutine);
            _pitchRoutine = null;
        }
    }
}
