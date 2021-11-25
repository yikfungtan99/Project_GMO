using UnityEngine;

public class HealBuffData : BuffData
{
    private IHealth health;
    private int healAmount;

    public HealBuffData(HealBuff buff, GameObject obj) : base(buff, obj)
    {
        health = obj.GetComponent<IHealth>();
        healAmount = buff.healAmount;
    }

    protected override void ApplyEffect()
    {
        health.Health += healAmount;
    }

    protected override void End()
    {
        
    }
}
