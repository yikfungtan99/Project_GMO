using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuffData : BuffData
{
    private WeaponComponent weapon;
    private int damageAmount;
    public DamageBuffData(DamageBuffObject buff, GameObject obj) : base(buff, obj)
    {
        damageAmount = buff.damageAmount;
        weapon = obj.GetComponentInChildren<WeaponComponent>();
    }
    protected override void ApplyEffect()
    {
        weapon.BuffDamageAmount(damageAmount);
    }
    protected override void End()
    {
        weapon.BuffDamageAmount(-damageAmount);
    }
}
