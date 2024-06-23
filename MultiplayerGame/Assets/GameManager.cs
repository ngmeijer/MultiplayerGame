using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CanvasController _canvasController;
    [SerializeField] private ResourceCollectorController _resourceCollector;
    
    private void Start()
    {
        _resourceCollector.Attach(_canvasController);
    }
}