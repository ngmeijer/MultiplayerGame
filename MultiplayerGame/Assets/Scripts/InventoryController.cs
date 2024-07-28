using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public struct ArmorEquipment
{
    public ArmorSettings HelmetData;
    public ArmorSettings UpperBodyData;
    public ArmorSettings LowerBodyData;

    public int TotalArmor;
    public float TotalDamageReductionPercentage;
}

public class InventoryController : MonoBehaviour
{
    private PlayerControls _controls;

    [SerializeField] private ArmorEquipment _armorEquipment;

    [SerializeField] private WeaponSettings _weaponEquipped;

    [SerializeField] private UnityEvent OnChangedInventoryEnabledState = new UnityEvent();
    [SerializeField] private UnityEvent<ArmorEquipment> OnChangedArmorEquipped = new UnityEvent<ArmorEquipment>();
    [SerializeField] private UnityEvent<WeaponSettings> OnChangedWeaponEquipped = new UnityEvent<WeaponSettings>();

    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void Start()
    {
        UpdateArmorStats();
        OnChangedArmorEquipped?.Invoke(_armorEquipment);
    }

    private void OnEnable()
    {
        _controls.UI.Enable();
        _controls.UI.EnableInventoryUI.performed += HandleInventoryUIActivation;
    }

    private void OnDisable()
    {
        _controls.UI.Disable();
        _controls.UI.EnableInventoryUI.performed -= HandleInventoryUIActivation;
    }

    private void HandleInventoryUIActivation(InputAction.CallbackContext obj)
    {
        OnChangedInventoryEnabledState?.Invoke();
    }

    private void UpdateArmorStats()
    {
        _armorEquipment.TotalArmor = (int)(_armorEquipment.HelmetData.ArmorValue + 
                                           _armorEquipment.UpperBodyData.ArmorValue +
                                           _armorEquipment.LowerBodyData.ArmorValue);
        _armorEquipment.TotalDamageReductionPercentage = _armorEquipment.TotalArmor / CombatController.MAX_ARMOR;
    }
}
