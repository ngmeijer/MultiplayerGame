using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : NetworkBehaviour
{
    [SerializeField] private MovementSettings _moveSettings;
    [SerializeField] private Transform _cameraHolder;
    private Transform _cameraTarget;
    private Camera _camera;
    private Vector3 _previousTargetPosition;
    private Vector2 _moveDirection;
    private PlayerControls _controls;
    private bool _inDash;
    private bool _inZoom;

    private float _zoomValue;

    public override void OnStartLocalPlayer()
    {
        _camera = Camera.main;
        _cameraTarget = this.transform;
        //_camera.transform.position += transform.position - _camera.transform.position;

        _controls = new PlayerControls();
        _controls.Player.Enable();
        _controls.Player.Zoom.performed += HandleZoomPerformed;
        _controls.Player.Zoom.canceled += HandleZoomCancelled;

        _previousTargetPosition = _cameraTarget.position;

        if (_camera == null)
            _camera = GetComponent<Camera>();
    }

    public override void OnStopLocalPlayer()
    {
        _controls.Player.Disable();
        _controls.Player.Zoom.performed -= HandleZoomPerformed;
        _controls.Player.Zoom.canceled -= HandleZoomCancelled;
    }

    private void Update()
    {
        HandleZoom();
    }

    private void LateUpdate()
    {
        HandleMove();
    }

    public void HandleMove()
    {
        Vector3 moveDelta = _cameraTarget.position - _previousTargetPosition;
        _camera.transform.position += moveDelta;
        _previousTargetPosition = _cameraTarget.position;
    }

    public void HandleDash(int pRemainingCharges)
    {
        StartCoroutine(InitializeDash(pRemainingCharges));
    }

    private void HandleZoom()
    {
        if (!_inZoom)
            return;
        
        float newSize = Mathf.Lerp(
            _camera.orthographicSize, 
            _camera.orthographicSize + (_moveSettings.ZoomSpeed * -_zoomValue),
            Time.deltaTime * _moveSettings.ZoomSpeed);

        newSize = Mathf.Clamp(newSize, _moveSettings.MaxZoomIn, _moveSettings.MaxZoomOut);
        _camera.orthographicSize = newSize;
    }

    private void HandleZoomPerformed(InputAction.CallbackContext callbackContext)
    {
        _inZoom = true;

        _zoomValue = callbackContext.ReadValue<float>();
    }

    private void HandleZoomCancelled(InputAction.CallbackContext callbackContext)
    {
        _inZoom = false;

        _zoomValue = 0;
    }

    private IEnumerator InitializeDash(int pRemainingCharges)
    {
        if (_inDash)
            yield break;

        if (pRemainingCharges <= 0)
            yield break;
        
        _inDash = true;
        
        yield return new WaitForSeconds(_moveSettings.DashDuration);

        _inDash = false;
    }
}
