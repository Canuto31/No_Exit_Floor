using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionDistance = 2.5f;
    
    private IInteractable _currentInteractable;
    private Transform _currentInteractableTransform;

    private bool _wasInteractable;

    [Header("UI")]
    public RectTransform interactIcon;
    public Vector3 iconOffset = new Vector3(0, 0.3f, 0);

    private void Update()
    {
        DetectInteractable();
        UpdateInteractIcon();
        PrintRaycast();
        DebugInteractionState();
    }

    private void DetectInteractable()
    {
        _currentInteractable = null;
        _currentInteractableTransform = null;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            _currentInteractable = hit.collider.GetComponent<IInteractable>();

            if (_currentInteractable != null)
            {
                _currentInteractableTransform = hit.collider.transform;
            }
        }
    }

    private void UpdateInteractIcon()
    {
        if (NoteReader.instance != null && NoteReader.instance.gameObject.activeSelf)
        {
            interactIcon.gameObject.SetActive(false);
            return;
        }
        
        if (_currentInteractableTransform == null)
        {
            interactIcon.gameObject.SetActive(false);
            return;
        }
        
        interactIcon.gameObject.SetActive(true);
        
        Vector3 worldPos = _currentInteractableTransform.position + iconOffset;
        Vector3 screedPos = playerCamera.WorldToScreenPoint(worldPos);

        if (screedPos.z < 0)
        {
            interactIcon.gameObject.SetActive(false);
            return;
        }
        
        interactIcon.position = screedPos;
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

        if (NoteReader.instance != null && NoteReader.instance.IsOpen())
        {
            NoteReader.instance.Close();
            return;
        }

        _currentInteractable?.Interact();
    }
}
