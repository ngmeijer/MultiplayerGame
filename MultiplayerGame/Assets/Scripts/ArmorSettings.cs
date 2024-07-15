using UnityEngine;

public enum ArmorType
{
    Helmet,
    UpperbodyArmor,
    LowerBodyArmor,
}

[CreateAssetMenu(menuName = "Player SO/Armor instance")]
public class ArmorSettings : ScriptableObject
{
    [SerializeField] private ArmorType _type;
    public ArmorType Type => _type;
    [SerializeField] private float _armorValue;
    public float ArmorValue => _armorValue;
    [SerializeField] private Sprite _armorUISprite;
    public Sprite ArmorUISprite => _armorUISprite;
}