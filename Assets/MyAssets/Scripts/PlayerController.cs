using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    
    private Rigidbody _rb;

    private Vector2 _inputVector;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = default;

        MovePlayerFirstPerson(moveDirection);
    }

    private void MovePlayerFirstPerson(Vector3 moveDirection)
    {
        moveDirection = (transform.forward * _inputVector.y) + (transform.right * _inputVector.x);
        _rb.MovePosition(_rb.position + moveDirection * (speed * Time.deltaTime));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            _inputVector = context.ReadValue<Vector2>();
        }
    }
}
