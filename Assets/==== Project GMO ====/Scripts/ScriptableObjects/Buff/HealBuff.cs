using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealBuff", menuName = "Project_GMO/Buff/HealBuff")]
public class HealBuff : BuffObject
{
    public int healAmount;

    public override BuffData InitializeBuff(GameObject obj)
    {
        return new HealBuffData(this, obj);
    }
}

public class HealBuffData : BuffData
{
    private IHaveHealth health;
    private int healAmount;

    public HealBuffData(HealBuff buff, GameObject obj) : base(buff, obj)
    {
        health = obj.GetComponent<IHaveHealth>();
        healAmount = buff.healAmount;
    }

    protected override void ApplyEffect()
    {
        health.Health += healAmount;
    }

    public override void End()
    {
        
    }
}
