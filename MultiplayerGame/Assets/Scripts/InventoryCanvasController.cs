using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryUI;
    private bool _inventoryUIEnabled;
    
    public void HandleInventoryCanvasState()
    {
        _inventoryUIEnabled = !_inventoryUIEnabled;
        _inventoryUI.SetActive(_inventoryUIEnabled);
    }
}
