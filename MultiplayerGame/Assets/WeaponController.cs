using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : NetworkBehaviour
{
    private PlayerControls _controls;
    private Camera _cam;
    private bool _canAttack = true;
    private bool _inScope;
    private Animator _currentWeaponAnimator;
    private WeaponSettings _weaponSettings;

    [SerializeField] private LineRenderer _scopeTracer;
    [SerializeField] private Transform _weaponParent;
    [SerializeField] private Transform _rangedWeaponParent;
    [SerializeField] private Transform _rangedWeaponLookAt;
    
    public override void OnStartLocalPlayer()
    {
        _cam = Camera.main;
        _scopeTracer.enabled = false;
        _controls = new PlayerControls();

        _controls.Player.Enable();
        _controls.Player.Attack.performed += HandleAttackPerformed;
        _controls.Player.MouseMove.performed += HandleMouseMovePerformed;
        _controls.Player.RangedWeaponScope.started += HandleWeaponScopeStarted;
        _controls.Player.RangedWeaponScope.canceled += HandleWeaponScopeCanceled;
    }

    public override void OnStopLocalPlayer()
    {
        _controls.Player.Disable();
        _controls.Player.Attack.performed -= HandleAttackPerformed;
        _controls.Player.MouseMove.performed -= HandleMouseMovePerformed;
        _controls.Player.RangedWeaponScope.started -= HandleWeaponScopeStarted;
        _controls.Player.RangedWeaponScope.canceled -= HandleWeaponScopeCanceled;
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;
        
        _scopeTracer.SetPosition(0, _weaponParent.position);
        _scopeTracer.SetPosition(1, _rangedWeaponLookAt.position);
    }

    public void ReceiveWeaponData(WeaponSettings pWeaponSettings)
    {
        _weaponSettings = pWeaponSettings;
        _currentWeaponAnimator = _weaponParent.GetComponentInChildren<Animator>();
    }

    private IEnumerator StartWeaponAttackCountdown()
    {
        yield return new WaitForSeconds(_weaponSettings.AttackSpeed);

        _canAttack = true;
    }

    private void HandleMouseMovePerformed(InputAction.CallbackContext obj)
    {
        Vector2 mousePos = obj.ReadValue<Vector2>();
        Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _cam.nearClipPlane));
        bool hasValidPositionOnGround = Physics.Raycast(mouseWorldPos, _cam.transform.forward);

        Vector3 lookAtTargetPos = Vector3.zero;
        if (!hasValidPositionOnGround)
            return;

        bool canSeePosition = Physics.Raycast(_weaponParent.position, mouseWorldPos, out RaycastHit visiblePositionData);
        if (!canSeePosition)
            lookAtTargetPos = visiblePositionData.point;
        

        lookAtTargetPos.y = _weaponParent.position.y;
        _rangedWeaponLookAt.transform.position = lookAtTargetPos;
        
        _rangedWeaponParent.LookAt(_rangedWeaponLookAt);
    }

    private void HandleAttackPerformed(InputAction.CallbackContext obj)
    {
        if (!_canAttack)
            return;

        if (_currentWeaponAnimator == null)
            return;
        
        _currentWeaponAnimator.SetTrigger("Attack");
        _canAttack = false;

        StartCoroutine(StartWeaponAttackCountdown());
    }

    private void HandleWeaponScopeStarted(InputAction.CallbackContext obj)
    {
        _scopeTracer.enabled = true;
        _inScope = true;
    }

    private void HandleWeaponScopeCanceled(InputAction.CallbackContext obj)
    {
        _scopeTracer.enabled = false;
        _inScope = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_rangedWeaponLookAt.position, 0.5f);
        Gizmos.DrawLine(transform.position, _rangedWeaponParent.position);
    }
}
