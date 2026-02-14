using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerWakeUpEffect : MonoBehaviour
{
    public Volume globalVolume;
    public float wakeUpDuration = 50f;

    private DepthOfField dof;

    private void Start()
    {
        globalVolume.profile.TryGet(out dof);
        StartCoroutine(WakeUpRoutine());
    }

    IEnumerator WakeUpRoutine()
    {
        float time = 0f;

        dof.gaussianMaxRadius.value = 1.5f;

        while (time < wakeUpDuration)
        {
            time += Time.deltaTime;
            float t = time / wakeUpDuration;

            dof.gaussianMaxRadius.value = Mathf.Lerp(1.5f, 0f, t);
            
            yield return null;
        }
        dof.gaussianMaxRadius.value = 0f;
    }
}
