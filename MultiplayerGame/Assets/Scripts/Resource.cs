using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ResourceType
{
    GOLD,
    IRON,
}

public class Resource : MonoBehaviour, IResource
{
    [SerializeField] private ResourceType _type;
    [SerializeField] private int _maxResourceAmount;
    [SerializeField] private float _collectionRate;

    [SerializeField] private TextMeshProUGUI _currentResourceAmountText;
    
    private float _currentAmount;

    private void Start()
    {
        _currentAmount = _maxResourceAmount;
        _currentResourceAmountText.SetText(_currentAmount.ToString());
    }


    public ResourceType GetResourceType() => _type;
    
    public float Collect()
    {
        float newAmount = _currentAmount - _collectionRate;
        float collectedAmount = _currentAmount - newAmount;
        _currentAmount = newAmount;
        
        _currentResourceAmountText.SetText(_currentAmount.ToString());
        
        if(_currentAmount <= 0)
            DestroyInstance();

        return collectedAmount;
    }

    public void DestroyInstance()
    {
        
    }
}

public interface IResource
{
    public ResourceType GetResourceType();
    
    public float Collect();

    public void DestroyInstance();
}
