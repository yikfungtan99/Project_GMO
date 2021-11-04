using UnityEngine;

public class MaxHealthBuffData : BuffData
{
    private IHaveHealth health;
    private int maxHealthIncrease;

    public MaxHealthBuffData(MaxHealthBuff buff, GameObject obj) : base(buff, obj)
    {
        health = obj.GetComponent<IHaveHealth>();
        maxHealthIncrease = buff.maxHealthIncrease;
    }
    protected override void ApplyEffect()
    {
        health.MaxHealth += maxHealthIncrease;
    }

    public override void End()
    {
        health.MaxHealth -= maxHealthIncrease;
    }
}