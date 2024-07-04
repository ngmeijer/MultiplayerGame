using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopController : MonoBehaviour, ISubject
{
    private List<IObserver> _observers = new List<IObserver>();

    [SerializeField] private GameObject _enableShopPopup;
    [SerializeField] private TextMeshProUGUI _enableShopPopupText;
    private PlayerControls _controls;

    private bool _inInteractionArea;

    private void Awake()
    {
        _controls = new PlayerControls();
        _controls.UI.EnableShopUI.started += EnableShopUI;
        _controls.Player.Enable();
    }

    private void EnableShopUI(InputAction.CallbackContext context)
    {
        Debug.Log(_inInteractionArea);
        if (!_inInteractionArea)
            return;
        HandleObjectState(true);
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }
    
    private void Start()
    {
        _controls.UI.Enable();

        string displayName = _controls.FindAction("EnableShopUI").GetBindingDisplayString();
        _enableShopPopupText.SetText($"Trade: {displayName}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MovementController moveController))
        {
            Debug.Log("player entered");
            _enableShopPopup.SetActive(true);
            _inInteractionArea = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MovementController moveController))
        {
            _enableShopPopup.SetActive(false);
            _inInteractionArea = false;
            HandleObjectState(false);
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

    public void HandleObjectState(bool pEnabled)
    {
        foreach (var observer in _observers)
        {
            observer.HandleObjectState(pEnabled);
        }
    }
}
