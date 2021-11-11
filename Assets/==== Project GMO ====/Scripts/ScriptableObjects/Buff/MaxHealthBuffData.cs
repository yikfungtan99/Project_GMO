using UnityEngine;

public class MaxHealthBuffData : BuffData
{
    private IBuffableHealth health;
    private int maxHealthModifyAmount;

    public MaxHealthBuffData(MaxHealthBuff buff, GameObject obj) : base(buff, obj)
    {
        health = obj.GetComponent<IBuffableHealth>();
        maxHealthModifyAmount = buff.maxHealthModifyAmount;
    }

    protected override void ApplyEffect()
    {
        health.AdditionalMaxHealth += maxHealthModifyAmount;
    }

    public override void End()
    {
        Debug.Log("End");
        health.AdditionalMaxHealth -= maxHealthModifyAmount;
    }
}