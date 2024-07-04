using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWorldCanvasController : MonoBehaviour
{
    [SerializeField] private MovementSettings _settings;
    [SerializeField] private GameObject _dashChargeUIPrefab;
    [SerializeField] private Transform _dashChargesUIParent;
    [SerializeField] private Color _dashChargeUIColor;
    private Color _currentDashChargeUIColor;
    private List<Image> _dashChargeImages = new List<Image>();

    private void Start()
    {
        _currentDashChargeUIColor = _dashChargeUIColor;
        CreateDashChargesUI();
    }

    private void CreateDashChargesUI()
    {
        for (int i = 0; i < _settings.MaxDashCharges; i++)
        {
            Image dashInstance = Instantiate(_dashChargeUIPrefab, _dashChargesUIParent).GetComponentInChildren<Image>();
            _dashChargeImages.Add(dashInstance);
        }   
    }

    public void UpdateDashChargesUI()
    {
        for (int i = 0; i < _dashChargeImages.Count; i++)
        {
            Image currentImage = _dashChargeImages[i];
            if (i <= _settings.RemainingDashCharges - 1)
            {
                _currentDashChargeUIColor.a = 1;
                currentImage.color = _currentDashChargeUIColor;
                continue;
            }

            _currentDashChargeUIColor.a = 0;
            currentImage.color = _currentDashChargeUIColor;
        }
    }
}
