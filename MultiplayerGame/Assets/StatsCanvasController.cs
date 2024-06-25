using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsCanvasController : MonoBehaviour, IObserver
{
    [SerializeField] private TextMeshProUGUI _goldCountText;
    [SerializeField] private TextMeshProUGUI _ironCountText;

    [SerializeField] private PlayerResourceStats _resourceStats;

    public void UpdateObserver(ISubject subject)
    {
        _goldCountText.SetText(_resourceStats.GoldAmount.ToString());
        _ironCountText.SetText(_resourceStats.IronAmount.ToString());
    }

    public void HandleObjectState(bool pEnabled)
    {
        
    }
}