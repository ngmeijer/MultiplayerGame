using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCanvasController : MonoBehaviour, IObserver
{
    [SerializeField] private GameObject _shopUI;
    
    public void UpdateObserver(ISubject subject)
    {
        
    }

    public void HandleObjectState(bool pEnabled)
    {
        _shopUI.SetActive(pEnabled);
    }
}
