using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] [Range(1, 20)] private float _moveSpeed;
    
    private Vector2 _moveDirection;
    private PlayerControls _controls;

    private void Awake()
    {
        _controls = new PlayerControls();
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Update()
    {
        _moveDirection = _controls.Player.Move.ReadValue<Vector2>();
        _moveDirection.Normalize();
        Vector3 moveDelta = new Vector3(_moveDirection.x, 0, _moveDirection.y) * _moveSpeed * Time.deltaTime;
        transform.position += moveDelta;
    }
}
