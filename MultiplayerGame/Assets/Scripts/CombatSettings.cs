using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Player SO/Combat")]
public class CombatSettings : ScriptableObject
{
    [Header("Health")]
    [SerializeField] [Range(1, 400)] private int _maxHealth;
    public int MaxHealth => _maxHealth;
    
    [SerializeField] [Range(1, 400)] private int _maxArmor;
    public int MaxArmor => _maxArmor;
    
    [SerializeField] [Range(0, 50)] private int _weaponRotationSpeed;
    public int WeaponRotationSpeed => _weaponRotationSpeed;
    
}