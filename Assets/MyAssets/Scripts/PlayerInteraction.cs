using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionDistance = 2.5f;
    
    private IInteractable _currentInteractable;

    private bool _wasInteractable;

    private void Update()
    {
        DetectInteractable();
        PrintRaycast();
        DebugInteractionState();
    }

    private void DetectInteractable()
    {
        _currentInteractable = null;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            _currentInteractable = hit.collider.GetComponent<IInteractable>();
        }
    }

    private void PrintRaycast()
    {
        Color rayColor = _currentInteractable != null ? Color.green : Color.red;
        
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactionDistance, rayColor);
    }

    private void DebugInteractionState()
    {
        bool isInteractable = _currentInteractable != null;

        if (isInteractable != _wasInteractable)
        {
            if (isInteractable)
            {
                Debug.Log("🟢 Puedo interactuar");
            }
            else
            {
                Debug.Log("🔴 No puedo interactuar");
            }

            _wasInteractable = isInteractable;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (NoteReader.instance != null && NoteReader.instance.gameObject.activeSelf)
            return;

        _currentInteractable?.Interact();
    }
}
