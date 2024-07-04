using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private StatsCanvasController _statsCanvasController;
    [SerializeField] private ResourceCollectorController _resourceCollector;

    [SerializeField] private ShopController _shopController;
    [SerializeField] private ShopCanvasController _shopCanvasController;
    
    private void Start()
    {
        _resourceCollector.Attach(_statsCanvasController);
        
        if(_shopController != null && _shopCanvasController != null)
            _shopController.Attach(_shopCanvasController);
    }
}