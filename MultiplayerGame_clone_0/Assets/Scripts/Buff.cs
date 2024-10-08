using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    HealthInstant,
    HealthRegen,
}

public enum BuffMeasurement
{
    Percentage,
    Absolute
}

[Serializable]
public struct BuffData
{
    public BuffType BuffType;
    public int BuffAmount;
    public BuffMeasurement BuffMeasurement;
    public float BuffTimespan;
    public float BuffInterval;
}

public class Buff : MonoBehaviour
{
    [SerializeField] private BuffData _data;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out IHealthHandler healthHandler))
        {
            healthHandler.DetermineBuffResult(_data);
            this.gameObject.SetActive(false);
        }
    }
}