using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class MovementController : MonoBehaviour, IMove
{
    [SerializeField] private MovementSettings _moveSettings;
    [SerializeField] private GameObject _gfx;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _lookAt;
    [SerializeField] private UnityEvent<int> OnDashCountChanged = new UnityEvent<int>();

    private Vector2 _moveDirection;
    private Vector3 _moveDelta;
    private PlayerControls _controls;
    private float _currentMoveSpeed;
    private bool _inDash;
    private bool _rechargingDash;
    private bool _resetDashRecharge;
    private int _remainingDashCharges;

    private void Awake()
    {
        _controls = new PlayerControls();
        
        if (_rb == null)
            _rb = GetComponent<Rigidbody>();
        
        _remainingDashCharges = _moveSettings.MaxDashCharges;
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
        _controls.Player.Move.performed += HandleMovePerformed;
        _controls.Player.Move.canceled += HandleMoveCancelled;
        _controls.Player.Dash.performed += HandleDash;
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
        _controls.Player.Move.performed -= HandleMovePerformed;
        _controls.Player.Move.canceled -= HandleMoveCancelled;
        _controls.Player.Dash.performed -= HandleDash;
    }

    private void Start()
    {
        _lookAt.localPosition = new Vector3(1, 0, 0);
        _currentMoveSpeed = _moveSettings.WalkSpeed;
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
        _moveDelta.x = _moveDirection.x;
        _moveDelta.z = _moveDirection.y;
        _moveDelta *= _currentMoveSpeed * Time.fixedDeltaTime;
        _rb.velocity = _moveDelta;
    }

    public void HandleDash(InputAction.CallbackContext callbackContext)
    {
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
        OnDashCountChanged?.Invoke(_remainingDashCharges);
        
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
        OnDashCountChanged?.Invoke(_remainingDashCharges);
        _rechargingDash = false;
        
        if(_remainingDashCharges < _moveSettings.MaxDashCharges)
            RechargeDash();
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
