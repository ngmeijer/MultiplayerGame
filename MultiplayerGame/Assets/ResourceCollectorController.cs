using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceCollectorController : MonoBehaviour, ISubject
{
    private IResource _currentSelectedResource;
    [SerializeField] private PlayerResourceStats _currentResources;

    private List<IObserver> _observers = new List<IObserver>();

    private void Start()
    {
        #if UNITY_EDITOR
            _currentResources.ResetValues();
        #endif
    }

    private IEnumerator collectResource()
    {
        yield return new WaitForSeconds(1);

        if (_currentSelectedResource == null)
            yield break;
        
        AddResource(_currentSelectedResource.GetResourceType(), _currentSelectedResource.Collect());
        
        UpdateObserver();
        
        StartCoroutine(collectResource());
    }

    private void AddResource(ResourceType pType, float pAmount)
    {
        switch (pType)
        {
            case ResourceType.GOLD:
                _currentResources.GoldAmount += pAmount;
                break;
            case ResourceType.IRON:
                _currentResources.IronAmount += pAmount;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IResource resourceInstance))
        {
            _currentSelectedResource = resourceInstance;
            StartCoroutine(collectResource());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IResource resourceInstance))
        {
            if (resourceInstance != _currentSelectedResource)
                return;
            
            StopCoroutine(collectResource());
            _currentSelectedResource = null;
        }
    }

    public void Attach(IObserver observer)
    {
        this._observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        this._observers.Remove(observer);
    }


    public void UpdateObserver()
    {
        foreach (var observer in _observers)
        {
            observer.UpdateObserver(this);
        }
    }
}
