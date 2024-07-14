using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    private PlayerControls _controls;

    [SerializeField] private UnityEvent OnChangedInventoryState = new UnityEvent();

    private void Awake()
    {
        _controls = new PlayerControls();
    }
    
    private void OnEnable()
    {
        _controls.UI.Enable();
        _controls.UI.EnableInventoryUI.performed += HandleMovePerformed;
    }

    private void OnDisable()
    {
        _controls.UI.Disable();
        _controls.UI.EnableInventoryUI.performed -= HandleMovePerformed;
    }

    private void HandleMovePerformed(InputAction.CallbackContext obj)
    {
        OnChangedInventoryState?.Invoke();
    }
}
