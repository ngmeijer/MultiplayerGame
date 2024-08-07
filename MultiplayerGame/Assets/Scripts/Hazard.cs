using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HazardType
{
    
}

public class Hazard : MonoBehaviour
{
    [SerializeField] private int _damage;
    private float _timer;
    [SerializeField] private float _damageInterval = 0.5f;

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}