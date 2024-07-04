using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour, IMove
{
    [SerializeField] private MovementSettings _moveSettings;
    private float _currentMoveSpeed;
    private Vector2 _moveDirection;
    private PlayerControls _controls;
    private bool _inDash;

    private void Awake()
    {
        _controls = new PlayerControls();
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
        _currentMoveSpeed = _moveSettings.WalkSpeed;
    }

    private void Update()
    {
        HandleMove();
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
        Vector3 moveDelta = new Vector3(_moveDirection.x, 0, _moveDirection.y) * _currentMoveSpeed * Time.deltaTime;
        transform.position += moveDelta;
    }

    public void HandleDash(InputAction.CallbackContext callbackContext)
    {
        StartCoroutine(InitializeDash());
    }
    
    private IEnumerator InitializeDash()
    {
        if (_inDash)
            yield break;

        if (_moveSettings.RemainingDashCharges <= 0)
            yield break;
        
        _inDash = true;

        _currentMoveSpeed = _moveSettings.DashSpeed;
        
        yield return new WaitForSeconds(_moveSettings.DashDuration);
        _currentMoveSpeed = _moveSettings.WalkSpeed;

        _inDash = false;
    }
}
