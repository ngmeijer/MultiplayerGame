using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private MovementSettings _moveSettings;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _cameraTarget;
    private Vector3 _previousTargetPosition;
    private Vector2 _moveDirection;
    private PlayerControls _controls;
    private bool _inDash;
    private bool _inZoom;

    private float _zoomValue;

    private void Awake()
    {
        _controls = new PlayerControls();
        if (_camera == null)
            _camera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
        _controls.Player.Zoom.performed += HandleZoomPerformed;
        _controls.Player.Zoom.canceled += HandleZoomCancelled;
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
        _controls.Player.Zoom.performed -= HandleZoomPerformed;
        _controls.Player.Zoom.canceled -= HandleZoomCancelled;
    }

    private void Start()
    {
        _previousTargetPosition = _cameraTarget.position;
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
        transform.position += moveDelta;
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
