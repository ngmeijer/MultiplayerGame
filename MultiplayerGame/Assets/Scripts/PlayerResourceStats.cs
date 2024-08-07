using UnityEngine;

[CreateAssetMenu(menuName = "Player SO/Resources")]
public class PlayerResourceStats : ScriptableObject
{
    public float GoldAmount;
    public float IronAmount;

    public void ResetValues()
    {
        GoldAmount = 0;
        IronAmount = 0;
    }
}