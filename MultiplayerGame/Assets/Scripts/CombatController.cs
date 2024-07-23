using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CombatController : MonoBehaviour, IHealthHandler
{
    [SerializeField] private CombatSettings _settings;
    [SerializeField] private UnityEvent<float> OnHealthChanged = new();
    [SerializeField] private Transform _weaponParent;
    private PlayerControls _controls;
    private Vector3 _startPos;
    private float _remainingHealth;
    private int _remainingArmor;
    private Animator _currentWeaponAnimator;

    private WeaponSettings _weaponSettings;

    private ArmorSettings _helmetSettings;
    private ArmorSettings _upperBodySettings;
    private ArmorSettings _lowerBodySettings;

    private bool _canAttack = true;

    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void Start()
    {
        _remainingHealth = _settings.MaxHealth;
        _startPos = transform.position;
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
        _controls.Player.Attack.performed += HandleAttackPerformed;
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
        _controls.Player.Attack.performed -= HandleAttackPerformed;
    }

    private IEnumerator StartWeaponAttackCountdown()
    {
        yield return new WaitForSeconds(_weaponSettings.AttackSpeed);

        _canAttack = true;
    }

    public void ReceiveDamage(int pAmount)
    {
        int netDamage = CalculateNetDamage(pAmount);
        
        _remainingHealth -= netDamage;

        _remainingHealth = Mathf.Clamp(_remainingHealth, 0, _settings.MaxHealth);
        
        OnHealthChanged?.Invoke(_remainingHealth);

        if (_remainingHealth == 0)
        {
            Debug.Log("Player died.");
            ResetStats();
        }
    }

    private int GetTotalAvailableArmor()
    {
        float helmetValue = _helmetSettings.ArmorValue;
        return (int)helmetValue;
    }

    private int CalculateNetDamage(int pRawDamage)
    {
        int totalAvailableArmor = GetTotalAvailableArmor();
        int damageReductionPercentage = totalAvailableArmor / _settings.MaxArmor;
        int netDamage = pRawDamage - (damageReductionPercentage * totalAvailableArmor);
        return netDamage;
    }

    private void HandleAttackPerformed(InputAction.CallbackContext obj)
    {
        if (!_canAttack)
            return;
        
        _currentWeaponAnimator.SetTrigger("Attack");
        _canAttack = false;

        StartCoroutine(StartWeaponAttackCountdown());
    }

    private void ResetStats()
    {
        transform.position = _startPos;
        _remainingHealth = _settings.MaxHealth;
        OnHealthChanged?.Invoke(_remainingHealth);
    }

    public void DetermineBuffResult(BuffData pData)
    {
        switch (pData.BuffType)
        {
            case BuffType.HealthInstant:
                GetInstantHealth(pData);
                break;
            case BuffType.HealthRegen:
                GetHealthRegen(pData);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pData.BuffType), pData.BuffType, null);
        }
    }

    public void ReceiveArmorData(ArmorSettings pArmorSettings)
    {
        switch (pArmorSettings.Type)
        {
            case ArmorType.Helmet:
                _helmetSettings = pArmorSettings;
                break;
            case ArmorType.UpperBodyArmor:
                _upperBodySettings = pArmorSettings;
                break;
            case ArmorType.LowerBodyArmor:
                _lowerBodySettings = pArmorSettings;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ReceiveWeaponData(WeaponSettings pWeaponSettings)
    {
        _weaponSettings = pWeaponSettings;
        _currentWeaponAnimator = _weaponParent.GetComponentInChildren<Animator>();
    }

    public void GetInstantHealth(BuffData pData)
    {
        switch (pData.BuffMeasurement)
        {
            case BuffMeasurement.Percentage:
                _remainingHealth += (pData.BuffAmount / 100f) * _settings.MaxHealth;
                break;
            case BuffMeasurement.Absolute:
                _remainingHealth += pData.BuffAmount;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pData.BuffMeasurement), pData.BuffMeasurement, null);
        }

        _remainingHealth = Mathf.Clamp(_remainingHealth, 0, _settings.MaxHealth);
        OnHealthChanged?.Invoke(_remainingHealth);
    }

    public void GetHealthRegen(BuffData pData)
    {
        float regenAmountPerInterval;
        switch (pData.BuffMeasurement)
        {
            case BuffMeasurement.Percentage:
                regenAmountPerInterval = ((pData.BuffAmount / 100) * _settings.MaxHealth) / pData.BuffTimespan;
                break;
            case BuffMeasurement.Absolute:
                regenAmountPerInterval = pData.BuffAmount / pData.BuffTimespan;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        StartCoroutine(StartHealthRegen(regenAmountPerInterval, pData.BuffInterval));
    }

    private IEnumerator StartHealthRegen(float pRegenAmountPerInterval, float pInterval)
    {
        while (_remainingHealth < _settings.MaxHealth)
        {
            _remainingHealth += pRegenAmountPerInterval;
            
            OnHealthChanged?.Invoke(_remainingHealth);
            
            yield return new WaitForSeconds(pInterval);
        }
    }

}
