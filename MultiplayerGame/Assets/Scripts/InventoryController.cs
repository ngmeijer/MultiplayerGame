using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    private PlayerControls _controls;

    [SerializeField] private ArmorSettings _helmetEquipped;
    [SerializeField] private ArmorSettings _upperBodyArmorEquipped;
    [SerializeField] private ArmorSettings _lowerBodyArmorEquipped;

    [SerializeField] private UnityEvent OnChangedInventoryEnabledState = new UnityEvent();
    [SerializeField] private UnityEvent<ArmorSettings> OnChangedArmorEquipped = new UnityEvent<ArmorSettings>();
    [SerializeField] private UnityEvent<WeaponSettings> OnChangedWeaponEquipped = new UnityEvent<WeaponSettings>();

    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void Start()
    {
        if (_helmetEquipped != null)
            OnChangedArmorEquipped?.Invoke(_helmetEquipped);
        if (_upperBodyArmorEquipped != null)
            OnChangedArmorEquipped?.Invoke(_upperBodyArmorEquipped);
        if (_lowerBodyArmorEquipped != null)
            OnChangedArmorEquipped?.Invoke(_lowerBodyArmorEquipped);
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
        OnChangedInventoryEnabledState?.Invoke();
    }
}
