using UnityEngine;

public enum WeaponType
{
    Shotgun,
    SMG, 
    Sniper,
    AssaultRifle,
    Sword,
    Axe
}

public enum WeaponRangeType
{
    Ranged,
    Melee,
}

[CreateAssetMenu(menuName = "Player SO/Weapon instance")]
public class WeaponSettings : ScriptableObject
{
    
    [SerializeField] private WeaponType _type;
    public WeaponType Type => _type;
    [SerializeField] private Sprite _weaponUISprite;
    public Sprite ArmorUISprite => _weaponUISprite;

    [SerializeField] private float _baseDamage;
    public float BaseDamage => _baseDamage;
    
    [SerializeField] private float _attackSpeed;
    public float AttackSpeed => _attackSpeed;
    
    [SerializeField] [Range(0, 100)] private float _critChance;
    public float CritChance => _critChance;

    [SerializeField] [Range(0, 1000)] private float _critDamage;
    public float CritDamage => _critDamage;

    [SerializeField] private GameObject _projectileInstance;
    public GameObject ProjectileInstance => _projectileInstance;
}