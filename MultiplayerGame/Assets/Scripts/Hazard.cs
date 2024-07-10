using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour, IHealthHandler
{
    [SerializeField] private int _damage;

    public void ReceiveDamage(int pAmount)
    {
        
    }

    public void RegenerateHealth(int pAmount)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out IHealthHandler damageHandler))
        {
            damageHandler.ReceiveDamage(_damage);
        }
    }
}

public interface IHealthHandler
{
    public void ReceiveDamage(int pAmount);

    public void RegenerateHealth(int pAmount);
}