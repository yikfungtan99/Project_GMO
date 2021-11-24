using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class WeaponComponent : ItemComponent
{
    [SerializeField] protected Transform weaponFireLocation;
    [SerializeField] protected Animator anim;

    public WeaponObject weaponSO;

    protected Attack primaryAttack;
    protected Attack secondaryAttack;

    protected WeaponRestrictor weaponRestrictor;

    protected float buffedAttackRate;

    private void Start()
    {
        weaponSO = (WeaponObject)itemObject;

        weaponRestrictor = GetComponent<WeaponRestrictor>();

        primaryAttack = weaponSO.primaryAttack;
        secondaryAttack = weaponSO.primaryAttack;
    }

    public virtual void PrimaryFireInput()
    {
        if (CanFire())
        {
            if(anim != null)
            {
                anim.speed = 1/ (primaryAttack.attackRate - (primaryAttack.attackRate * (buffedAttackRate / 100)));
                anim.Play("PrimaryFire");
            }
        }
    }
    protected virtual bool CanFire()
    {
        bool canFire = true;

        if(weaponRestrictor != null)
        {
            canFire = !weaponRestrictor.RestrictFire();
        }

        return canFire;
    }

    public void BuffAttackPercent(float attackPercent)
    {
        buffedAttackRate = attackPercent;
    }
    public abstract void PrimaryFire();
    public abstract void SecondaryAttack();
    protected abstract void SpawnAttack(GameObject attackObject, Transform attackPosition, float projectileSpeed);
    public virtual void AnimPrimaryFire()
    {
        PrimaryFire();
    }
}
