using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasController : MonoBehaviour, IObserver
{
    [SerializeField] private TextMeshProUGUI _goldCountText;
    [SerializeField] private TextMeshProUGUI _ironCountText;

    [SerializeField] private PlayerResourceStats _resourceStats;

    public void UpdateObserver(ISubject subject)
    {
        _goldCountText.SetText(_resourceStats.GoldAmount.ToString());
        _ironCountText.SetText(_resourceStats.IronAmount.ToString());
    }
}

public interface IObserver
{
    void UpdateObserver(ISubject subject);
}

public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}