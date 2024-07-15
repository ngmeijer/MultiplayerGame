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
            case ArmorType.UpperbodyArmor:
                _upperbodyArmorImage.sprite = pNewArmor.ArmorUISprite;
                break;
            case ArmorType.LowerBodyArmor:
                _lowerbodyArmorImage.sprite = pNewArmor.ArmorUISprite;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
