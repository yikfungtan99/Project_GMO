using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class WeaponComponent : ItemComponent
{
    [SerializeField] protected Transform weaponFireLocation;
    [SerializeField] protected Animator anim;
    protected AudioSource audioSource;

    public WeaponObject weaponSO;

    protected Attack primaryAttack;
    protected Attack secondaryAttack;

    protected WeaponRestrictor weaponRestrictor;

    protected float buffedAttackRate;

    public AudioClip attackAudio;

    private void Start()
    {
        weaponSO = (WeaponObject)itemObject;

        weaponRestrictor = GetComponent<WeaponRestrictor>();
        audioSource = GetComponent<AudioSource>();

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

    public void BuffDamageAmount(int damage)
    {
        primaryAttack.damage.DamageAmount += damage;
    }

    public void BuffAttackPercent(float attackPercent)
    {
        buffedAttackRate += attackPercent;
    }
    public abstract void PrimaryFire();
    public abstract void SecondaryAttack();
    protected abstract void SpawnAttack(GameObject attackObject, Transform attackPosition, float projectileSpeed);
    public virtual void AnimPrimaryFire()
    {
        PlayAttackAudio();
        PrimaryFire();
    }

    protected void PlayAttackAudio()
    {
        if(audioSource == null)
        {
            Debug.LogWarning(name + " do not have an audio source!");
            return;
        }

        audioSource.PlayOneShot(attackAudio);
    }
}
