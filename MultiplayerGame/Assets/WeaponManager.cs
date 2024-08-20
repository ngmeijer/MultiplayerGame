using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : NetworkBehaviour
{
    private PlayerControls _controls;
    private Camera _cam;
    private bool _canAttack = true;
    private bool _inScope;

    private Vector3 _mouseWorldPos;

    [SerializeField] private LineRenderer _scopeTracer;
    [SerializeField] private Transform _weaponParent;
    [SerializeField] private Transform _rangedWeaponParent;
    [SerializeField] private Transform _rangedWeaponLookAt;
    [SerializeField] private Transform _weaponBarrelEnd;
    private WeaponController _weaponController;
    
    public override void OnStartLocalPlayer()
    {
        _cam = Camera.main;
        _scopeTracer.enabled = false;
        _rangedWeaponLookAt.parent = null;
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

    private void LateUpdate()
    {
        if (!isLocalPlayer)
            return;
        
        _scopeTracer.SetPosition(0, _weaponBarrelEnd.position);
        _scopeTracer.SetPosition(1, new Vector3(_rangedWeaponLookAt.position.x, _weaponBarrelEnd.position.y, _rangedWeaponLookAt.position.z));
    }

    private void DetermineAimDirection(Vector2 pMousePos)
    {
        _mouseWorldPos = _cam.ScreenToWorldPoint(new Vector3(pMousePos.x, pMousePos.y, _cam.nearClipPlane));
        Physics.Raycast(_mouseWorldPos, _cam.transform.forward, out RaycastHit visiblePositionData);
        Vector3 lookAtTargetPos = visiblePositionData.point;
        lookAtTargetPos.y = _weaponParent.position.y;
        _rangedWeaponLookAt.transform.position = lookAtTargetPos;
        _rangedWeaponParent.LookAt(_rangedWeaponLookAt);
    }

    private void HandleMouseMovePerformed(InputAction.CallbackContext obj)
    {
        Vector2 mousePos = obj.ReadValue<Vector2>();
        DetermineAimDirection(mousePos);
    }

    private void HandleAttackPerformed(InputAction.CallbackContext obj)
    {
        _weaponController.StartAttack();
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
        
        Gizmos.DrawSphere(_mouseWorldPos, 0.5f);
    }
}
