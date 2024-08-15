using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private Image _helmetImage;
    [SerializeField] private Image _upperbodyArmorImage;
    [SerializeField] private Image _lowerbodyArmorImage;
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

    public void UpdateInventoryCanvas(ArmorSettings pNewArmor)
    {
        switch (pNewArmor.Type)
        {
            case ArmorType.Helmet:
                _helmetImage.sprite = pNewArmor.ArmorUISprite;
                break;
            case ArmorType.UpperBodyArmor:
                _upperbodyArmorImage.sprite = pNewArmor.ArmorUISprite;
                break;
            case ArmorType.LowerBodyArmor:
                _lowerbodyArmorImage.sprite = pNewArmor.ArmorUISprite;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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
