using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct ScreenShakeSettings
{
    public float Strength;
    public int Vibrato;
    public float Randomness;
}

public class CameraController : NetworkBehaviour
{
    [SerializeField] private MovementSettings _moveSettings;
    [SerializeField] private Vector3 _defaultOffset;
    [SerializeField] private ScreenShakeSettings _screenshakeSettings;
    private MovementController _moveController;
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
        _moveController = GetComponent<MovementController>();
        _camera = Camera.main;
        _cameraTarget = this.transform;

        //Figure out Y & Z difference and add it to position
        _camera.transform.parent.position = _cameraTarget.position + _defaultOffset;

        _controls = new PlayerControls();
        _controls.Player.Enable();
        _controls.Player.Zoom.performed += HandleZoomPerformed;
        _controls.Player.Zoom.canceled += HandleZoomCancelled;
        _controls.Player.Dash.performed += HandleDashPerformed;

        _previousTargetPosition = _cameraTarget.position;
    }

    public override void OnStopLocalPlayer()
    {
        _controls.Player.Disable();
        _controls.Player.Zoom.performed -= HandleZoomPerformed;
        _controls.Player.Zoom.canceled -= HandleZoomCancelled;
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;
        
        HandleZoom();
    }

    private void LateUpdate()
    {
        if (!isLocalPlayer)
            return;
        
        HandleMove();
    }

    public void HandleMove()
    {
        Vector3 moveDelta = _cameraTarget.position - _previousTargetPosition;
        _camera.transform.parent.position += moveDelta;
        _previousTargetPosition = _cameraTarget.position;
    }

    public void HandleDashPerformed(InputAction.CallbackContext callbackContext)
    {
        StartCoroutine(InitializeDash());
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

    private IEnumerator InitializeDash()
    {
        if (_inDash)
            yield break;

        if (_moveController.RemainingDashCharges <= 0)
            yield break;

        _inDash = true;
        InitializeScreenshake();
        
        yield return new WaitForSeconds(_moveSettings.DashDuration);

        _inDash = false;
    }

    public void InitializeScreenshake()
    {
        _camera.transform.DOShakePosition(0.9f * _moveSettings.DashDuration, _screenshakeSettings.Strength, _screenshakeSettings.Vibrato, _screenshakeSettings.Randomness);
    }
}
