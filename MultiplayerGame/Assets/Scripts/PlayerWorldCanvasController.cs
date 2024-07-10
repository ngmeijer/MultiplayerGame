using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWorldCanvasController : MonoBehaviour
{
    [SerializeField] private MovementSettings _moveSettings;
    [SerializeField] private GameObject _dashChargeUIPrefab;
    [SerializeField] private Transform _dashChargesUIParent;
    [SerializeField] private Color _dashChargeUIColor;
    private Color _currentDashChargeUIColor;
    private List<Image> _dashChargeImages = new List<Image>();

    [SerializeField] private CombatSettings _combatSettings;
    [SerializeField] private Image _healthBar;
    
    private void Start()
    {
        _currentDashChargeUIColor = _dashChargeUIColor;
        CreateDashChargesUI();
    }

    private void CreateDashChargesUI()
    {
        for (int i = 0; i < _moveSettings.MaxDashCharges; i++)
        {
            Image dashInstance = Instantiate(_dashChargeUIPrefab, _dashChargesUIParent).GetComponentInChildren<Image>();
            _dashChargeImages.Add(dashInstance);
        }   
    }

    public void UpdateDashChargesUI(int pRemainingCharges)
    {
        for (int i = 0; i < _dashChargeImages.Count; i++)
        {
            Image currentImage = _dashChargeImages[i];
            if (i <= pRemainingCharges - 1)
            {
                _currentDashChargeUIColor.a = 1;
                currentImage.color = _currentDashChargeUIColor;
                continue;
            }

            _currentDashChargeUIColor.a = 0;
            currentImage.color = _currentDashChargeUIColor;
        }
    }

    public void UpdateHealth(int pRemainingHealth)
    {
        Vector3 tempScale = _healthBar.transform.localScale;
        tempScale.x = pRemainingHealth / (float)_combatSettings.MaxHealth;
        _healthBar.transform.localScale = tempScale;
    }

    public void UpdateArmor()
    {
        
    }
}
