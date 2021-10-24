using UnityEngine;

[System.Serializable]
public class DamageType
{

}

[System.Serializable]
public class Damage
{
    [SerializeField] private int damageAmount = 0;
    [SerializeField] private DamageType damageType;

    public Damage(int damageAmount, DamageType damageType)
    {
        this.damageAmount = damageAmount;
        this.damageType = damageType;
    }

    public int DamageAmount { get => damageAmount; set => damageAmount = value; }
    public DamageType DamageType { get => damageType; set => damageType = value; }
}
