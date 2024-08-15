using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class MovementController : NetworkBehaviour, IMove
{
    [SerializeField] private MovementSettings _moveSettings;
    [SerializeField] private GameObject _gfx;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _lookAt;
    [SerializeField] private UnityEvent<int> OnDash = new UnityEvent<int>();

    private Vector2 _moveDirection;
    private Vector3 _moveDelta;
    private PlayerControls _controls;
    private float _currentMoveSpeed;
    private bool _inDash;
    private bool _rechargingDash;
    private bool _resetDashRecharge;
    private int _remainingDashCharges;
    public int RemainingDashCharges => _remainingDashCharges;
    private bool _inUI;

    public override void OnStartLocalPlayer()
    {
        _lookAt.localPosition = new Vector3(1, 0, 0);
        _currentMoveSpeed = _moveSettings.WalkSpeed;
        
        _controls = new PlayerControls();
        
        _controls.Player.Enable();
        _controls.Player.Move.performed += HandleMovePerformed;
        _controls.Player.Move.canceled += HandleMoveCancelled;
        _controls.Player.Dash.performed += HandleDash;
        
        _controls.UI.Enable();
        _controls.UI.EnableInventoryUI.performed += HandleControlsDisabledInUI;
        
        if (_rb == null)
            _rb = GetComponent<Rigidbody>();
        
        _remainingDashCharges = _moveSettings.MaxDashCharges;
    }

    public override void OnStopLocalPlayer()
    {
        _controls.Player.Disable();
        _controls.Player.Move.performed -= HandleMovePerformed;
        _controls.Player.Move.canceled -= HandleMoveCancelled;
        _controls.Player.Dash.performed -= HandleDash;
        
        _controls.UI.Disable();
        _controls.UI.EnableInventoryUI.performed -= HandleControlsDisabledInUI;

        _controls = null;
    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    private void Update()
    {
        RotatePlayerToViewDirection();
    }

    private void RotatePlayerToViewDirection()
    {
        Vector3 lookAtPos = Vector3.zero;
        lookAtPos = Vector3.Lerp(_lookAt.localPosition,
            new Vector3(_moveDirection.x, _lookAt.localPosition.y, _moveDirection.y),
            Time.deltaTime * _moveSettings.RotateSpeed);

        _lookAt.localPosition = lookAtPos;
        _gfx.transform.LookAt(_lookAt.position, transform.up);
    }

    public void HandleMoveCancelled(InputAction.CallbackContext ctx)
    {
        _moveDirection = Vector2.zero;
    }

    public void HandleMovePerformed(InputAction.CallbackContext ctx)
    {
        _moveDirection = ctx.ReadValue<Vector2>();
        _moveDirection.Normalize();
    }

    public void HandleMove()
    {
        if (_inUI)
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        _moveDelta.x = _moveDirection.x;
        _moveDelta.z = _moveDirection.y;
        _moveDelta *= _currentMoveSpeed * Time.fixedDeltaTime;
        _rb.velocity = _moveDelta;
    }

    public void HandleDash(InputAction.CallbackContext callbackContext)
    {
        if (_inUI)
            return;
        
        StartCoroutine(InitializeDashRoutine());
    }

    private IEnumerator InitializeDashRoutine()
    {
        if (_inDash)
            yield break;

        if (_remainingDashCharges <= 0)
            yield break;

        _remainingDashCharges--;

        _inDash = true;

        _currentMoveSpeed = _moveSettings.DashSpeed;
        OnDash?.Invoke(_remainingDashCharges);
        
        yield return new WaitForSeconds(_moveSettings.DashDuration);
        _currentMoveSpeed = _moveSettings.WalkSpeed;

        _inDash = false;
        
        RechargeDash();
    }

    private void RechargeDash()
    {
        StartCoroutine(InitializeDashRechargeRoutine());
    }

    private IEnumerator InitializeDashRechargeRoutine()
    {
        if (_rechargingDash)
            yield break;
        
        _rechargingDash = true;
        yield return new WaitForSeconds(_moveSettings.DashRechargeRate);

        _remainingDashCharges++;
        OnDash?.Invoke(_remainingDashCharges);
        _rechargingDash = false;
        
        if(_remainingDashCharges < _moveSettings.MaxDashCharges)
            RechargeDash();
    }

    private void HandleControlsDisabledInUI(InputAction.CallbackContext obj)
    {
        _inUI = !_inUI;
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
