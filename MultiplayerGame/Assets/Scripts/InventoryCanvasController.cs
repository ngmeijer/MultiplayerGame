using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private Image _helmetImage;
    [SerializeField] private TextMeshProUGUI _helmetArmorValue;
    
    [SerializeField] private Image _upperbodyArmorImage;
    [SerializeField] private TextMeshProUGUI _upperbodyArmorValue;
    
    [SerializeField] private Image _lowerbodyArmorImage;
    [SerializeField] private TextMeshProUGUI _lowerbodyArmorValue;

    [SerializeField] private TextMeshProUGUI _totalArmorText;
    [SerializeField] private TextMeshProUGUI _damageReductionText;
    
    private bool _inventoryUIEnabled;

    private void Start()
    {
        _inventoryUI.SetActive(false);
    }

    public void HandleInventoryCanvasState()
    {
        _inventoryUIEnabled = !_inventoryUIEnabled;
        _inventoryUI.SetActive(_inventoryUIEnabled);
    }

    public void UpdateArmorInventoryCanvas(ArmorEquipment pArmorData)
    {
        UpdateArmorUI(pArmorData.HelmetData, _helmetImage, _helmetArmorValue);
        UpdateArmorUI(pArmorData.UpperBodyData, _upperbodyArmorImage, _upperbodyArmorValue);
        UpdateArmorUI(pArmorData.LowerBodyData, _lowerbodyArmorImage, _lowerbodyArmorValue);
        _totalArmorText.SetText($"Raw total armor: {pArmorData.TotalArmor}");
        _damageReductionText.SetText($"Damage reduction: {pArmorData.TotalDamageReductionPercentage * 100}%");
    }

    private void UpdateArmorUI(ArmorSettings pData, Image pImage, TextMeshProUGUI pArmorValue)
    {
        pImage.sprite = pData.ArmorUISprite;
        pArmorValue.SetText(pData.ArmorValue.ToString());
    }
    
    public void UpdateInventoryCanvas(WeaponSettings pNewWeapon)
    {
        switch (pNewWeapon.Type)
        {
            case WeaponType.Shotgun:
                break;
            case WeaponType.SMG:
                break;
            case WeaponType.Sniper:
                break;
            case WeaponType.AssaultRifle:
                break;
            case WeaponType.Sword:
                break;
            case WeaponType.Axe:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
