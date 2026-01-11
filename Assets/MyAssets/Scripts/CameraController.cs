using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 0.3f;
    
    private float _xRotation = 0f;
    
    private Vector2 _lookInput;

    public Transform player;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = _lookInput.x * sensitivity;
        float mouseY = _lookInput.y * sensitivity;
        
        RotateCamera(mouseX, mouseY);
        
        _lookInput = Vector2.zero;
    }

    private void RotateCamera(float mouseX, float mouseY)
    {
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }
    
    public void Onlook(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            _lookInput = context.ReadValue<Vector2>();
        }
    }
}
