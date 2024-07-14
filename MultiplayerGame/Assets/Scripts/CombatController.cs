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
                GetInstantHealth(pData.BuffAmount);
                break;
            case BuffType.HealthRegen:
                GetHealthRegen(pData.BuffAmount, pData.BuffTimespan, pData.BuffInterval);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pData.BuffType), pData.BuffType, null);
        }
    }

    public void GetInstantHealth(int pAmount)
    {
        _remainingHealth += pAmount;
        _remainingHealth = Mathf.Clamp(_remainingHealth, 0, _settings.MaxHealth);
        OnHealthChanged?.Invoke(_remainingHealth);
    }

    public void GetHealthRegen(int pAmount, float pTime, float pInterval)
    {
        StartCoroutine(StartHealthRegen(pAmount, pTime, pInterval));
    }

    private IEnumerator StartHealthRegen(float pTotalHealthAmount, float pTotalTimeForRegen, float pInterval)
    {
        float regenAmountPerInterval = pTotalHealthAmount / pTotalTimeForRegen;
        while (_remainingHealth < _settings.MaxHealth)
        {
            _remainingHealth += regenAmountPerInterval;
            
            OnHealthChanged?.Invoke(_remainingHealth);
            
            yield return new WaitForSeconds(pInterval);
        }
    }

}
