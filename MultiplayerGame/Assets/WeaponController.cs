using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private WeaponSettings _weaponSettings;
    public WeaponSettings Settings => _weaponSettings;
    private bool _canAttack = true;
    
    public bool CanAttack()
    {
        return _canAttack;
    }

    public void StartAttack()
    {
        if (!CanAttack())
            return;
        
        
    }

    private void PerformRangedAttack()
    {
        
    }
    
    private IEnumerator StartWeaponAttackCountdown()
    {
        yield return new WaitForSeconds(Settings.AttackSpeed);

        _canAttack = true;
    }
}
