using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Player SO/Movement")]
public class MovementSettings : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float _rotateSpeed;
    public float RotateSpeed => _rotateSpeed;

    [SerializeField] [Range(1, 400)] private float _walkSpeed;
    public float WalkSpeed => _walkSpeed;
    
    [Header("Dash")]
    [SerializeField] [Range(5, 1500)] private float _dashSpeed;
    public float DashSpeed => _dashSpeed;
    
    [SerializeField] [Range(0, 3)] private float _dashDuration;
    public float DashDuration => _dashDuration;

    [SerializeField] [Range(0, 3)] private int _maxDashCharges;
    public int MaxDashCharges => _maxDashCharges;

    [SerializeField] [Range(0, 3)] private float _dashRechargeRate;
    public float DashRechargeRate => _dashRechargeRate;
    
    [Header("Camera")] 
    [SerializeField] [Range(4, 10)] private float _maxZoomIn;
    public float MaxZoomIn => _maxZoomIn;
    
    [SerializeField] [Range(10, 50)] private float _maxZoomOut;
    public float MaxZoomOut => _maxZoomOut;

    [SerializeField] [Range(1, 10)] private float _zoomSpeed;
    public float ZoomSpeed => _zoomSpeed;
}