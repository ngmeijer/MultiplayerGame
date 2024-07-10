using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour, IHealthHandler
{
    [SerializeField] private CombatSettings _settings;
    private int _remainingHealth;
    private int _remainingArmor;

    private void Start()
    {
        _remainingHealth = _settings.MaxHealth;
    }

    public void ReceiveDamage(int pAmount)
    {
        _remainingHealth -= pAmount;

        if (_remainingHealth <= 0)
        {
            Debug.Log("Player died.");
        }
    }

    public void RegenerateHealth(int pAmount)
    {
        
    }
}
