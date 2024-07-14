using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatController : MonoBehaviour, IHealthHandler
{
    [SerializeField] private CombatSettings _settings;
    [SerializeField] private UnityEvent<float> OnHealthChanged = new();

    private Vector3 _startPos;

    private float _remainingHealth;
    private int _remainingArmor;
    
    private void Start()
    {
        _remainingHealth = _settings.MaxHealth;
        _startPos = transform.position;
    }

    public void ReceiveDamage(int pAmount)
    {
        _remainingHealth -= pAmount;

        _remainingHealth = Mathf.Clamp(_remainingHealth, 0, _settings.MaxHealth);
        
        OnHealthChanged?.Invoke(_remainingHealth);

        if (_remainingHealth == 0)
        {
            Debug.Log("Player died.");
            ResetStats();
        }
    }

    private void ResetStats()
    {
        transform.position = _startPos;
        _remainingHealth = _settings.MaxHealth;
        OnHealthChanged?.Invoke(_remainingHealth);
    }

    public void DetermineHazardResult()
    {
        
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
