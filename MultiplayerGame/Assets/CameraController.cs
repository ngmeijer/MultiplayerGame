using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] [Range(1, 20)] private float _moveSpeed;
    [SerializeField] private InputAction _cameraControls;

    private Vector2 _moveDirection;
    
    private void Start()
    {
        _cameraControls.Enable();
    }

    private void OnDisable()
    {
        _cameraControls.Disable();
    }

    private void Update()
    {
        _moveDirection = _cameraControls.ReadValue<Vector2>();
        _moveDirection.Normalize();
        transform.position += new Vector3(_moveDirection.x, 0, _moveDirection.y) * _moveSpeed * Time.deltaTime;
    }
}
