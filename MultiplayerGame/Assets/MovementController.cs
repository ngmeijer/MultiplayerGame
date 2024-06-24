using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField] [Range(1, 20)] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private GameObject _gfx;
    [SerializeField] private Transform _lookAt;

    private Vector2 _moveDirection;
    private PlayerControls _controls;

    private void Awake()
    {
        _controls = new PlayerControls();
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }

    private void Start()
    {
        _lookAt.localPosition = new Vector3(1, 0, 0);
    }

    private void Update()
    {
        movePlayer();
        rotatePlayerToViewDirection();
    }

    private void rotatePlayerToViewDirection()
    {
        Vector3 lookatPos = Vector3.zero;
        //Lerp position of lookat depending on input values
        lookatPos = Vector3.Lerp(_lookAt.localPosition, new Vector3(_moveDirection.x, 0, _moveDirection.y), Time.deltaTime * _rotateSpeed);
        
        //If no input, lerp lookat back to origin
        if(_moveDirection.magnitude == 0)
            lookatPos = Vector3.Lerp(_lookAt.localPosition, Vector3.zero, Time.deltaTime * _rotateSpeed);

        _lookAt.localPosition = lookatPos;
        _gfx.transform.LookAt(_lookAt.position, transform.up);
    }

    private void movePlayer()
    {
        _moveDirection = _controls.Player.Move.ReadValue<Vector2>();
        _moveDirection.Normalize();
        Vector3 moveDelta = new Vector3(_moveDirection.x, 0, _moveDirection.y) * _moveSpeed * Time.deltaTime;
        transform.position += moveDelta;
    }

    private void OnDrawGizmos()
    {
        if (_lookAt != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(_lookAt.position, 0.3f);
        }
    }
}
