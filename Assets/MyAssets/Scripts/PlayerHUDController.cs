using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHUDController : MonoBehaviour
{
    [Header("HUD Root")] 
    public GameObject hudRoot;
    
    [Header("Fade Settings")] 
    public float visibleTime = 3f;
    public float fadeDuration = 1f;
    
    private CanvasGroup _canvasGroup;
    private Coroutine _fadeRoutine;
    
    private FlashlightController _flashlight;

    private void Awake()
    {
        _canvasGroup = hudRoot.GetComponent<CanvasGroup>();
        hudRoot.SetActive(false);
        _canvasGroup.alpha = 0f;
    }

    public void OnToggleUI(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (_flashlight == null)
        {
            _flashlight = FlashlightController.instance;
            return;
        }

        ShowHUD();
    }

    private void ShowHUD()
    {
        hudRoot.SetActive(true);
        _canvasGroup.alpha = 1f;
        if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(AutoHideRoutine());
    }

    private IEnumerator AutoHideRoutine()
    {
        yield return new WaitForSeconds(visibleTime);
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        _canvasGroup.alpha = 0f;
        hudRoot.SetActive(false);
    }
}