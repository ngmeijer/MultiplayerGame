using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWorldCanvasController : NetworkBehaviour
{
    [SerializeField] private MovementSettings _moveSettings;
    [SerializeField] private GameObject _dashChargeUIPrefab;
    [SerializeField] private Transform _dashCharges;
    [SerializeField] private Transform _dashChargesUIContainer;
    [SerializeField] private Color _dashChargeUIColor;
    private Color _currentDashChargeUIColor;
    private List<Image> _dashChargeImages = new List<Image>();

    [SerializeField] private CombatSettings _combatSettings;
    [SerializeField] private Image _healthBar;
    
    public override void OnStartLocalPlayer()
    {
        _dashChargesUIContainer.gameObject.SetActive(true);
        _currentDashChargeUIColor = _dashChargeUIColor;
        CreateDashChargesUI();
    }

    private void CreateDashChargesUI()
    {
        for (int i = 0; i < _moveSettings.MaxDashCharges; i++)
        {
            Image dashInstance = Instantiate(_dashChargeUIPrefab, _dashCharges).GetComponentInChildren<Image>();
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

    public void UpdateHealth(float pRemainingHealth)
    {
        Vector3 tempScale = _healthBar.transform.localScale;
        tempScale.x = pRemainingHealth / _combatSettings.MaxHealth;
        _healthBar.transform.localScale = tempScale;
    }

    public void UpdateArmor()
    {
        
    }
}
