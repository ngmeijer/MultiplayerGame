using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Player SO/Movement")]
public class MovementSettings : ScriptableObject
{
    [SerializeField] private float _rotateSpeed;
    public float RotateSpeed => _rotateSpeed;

    [SerializeField] [Range(1, 20)] private float _walkSpeed;
    public float WalkSpeed => _walkSpeed;
    
    [SerializeField] [Range(5, 100)] private float _dashSpeed;
    public float DashSpeed => _dashSpeed;
    
    [SerializeField] [Range(0, 3)] private float _dashDuration;
    public float DashDuration => _dashDuration;

    [SerializeField] [Range(0, 3)] private int _maxDashCharges;
    public int MaxDashCharges => _maxDashCharges;

    [SerializeField] [Range(0, 3)] private float _dashRechargeRate;
    public float DashRechargeRate => _dashRechargeRate;

    public int RemainingDashCharges;

    private void OnEnable()
    {
        RemainingDashCharges = _maxDashCharges;
    }
}