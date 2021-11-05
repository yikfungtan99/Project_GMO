using UnityEngine;

public class MaxHealthBuffData : BuffData
{
    private IHaveHealth health;
    private int maxHealthModifyAmount;

    public MaxHealthBuffData(MaxHealthBuff buff, GameObject obj) : base(buff, obj)
    {
        health = obj.GetComponent<IHaveHealth>();
        maxHealthModifyAmount = buff.maxHealthModdifyAmount;
    }
    protected override void ApplyEffect()
    {
        health.MaxHealth += maxHealthModifyAmount;
    }

    public override void End()
    {
        health.MaxHealth -= maxHealthModifyAmount;
    }
}