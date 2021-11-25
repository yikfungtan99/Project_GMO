using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedBuffData : BuffData
{
    private WeaponComponent weapon;

    [Range(1, 100)]
    private float speedAmount;
    public AttackSpeedBuffData(AttackSpeedBuffObject buff, GameObject obj) : base(buff, obj)
    {
        weapon = obj.GetComponentInChildren<WeaponComponent>();
        speedAmount = buff.speedAmount;
    }
    protected override void ApplyEffect()
    {
        weapon.BuffAttackPercent(speedAmount);
    }
    protected override void End()
    {
        weapon.BuffAttackPercent(-speedAmount);
    }
}
