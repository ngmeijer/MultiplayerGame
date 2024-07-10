using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatController : MonoBehaviour, IHealthHandler
{
    [SerializeField] private CombatSettings _settings;
    [SerializeField] private UnityEvent<int> OnHealthChanged = new UnityEvent<int>();

    private int _remainingHealth;
    private int _remainingArmor;
    
    private void Start()
    {
        _remainingHealth = _settings.MaxHealth;
    }

    public void ReceiveDamage(int pAmount)
    {
        _remainingHealth -= pAmount;
        
        OnHealthChanged?.Invoke(_remainingHealth);

        if (_remainingHealth <= 0)
        {
            Debug.Log("Player died.");
        }
    }

    public void RegenerateHealth(int pAmount)
    {
        
    }
}
