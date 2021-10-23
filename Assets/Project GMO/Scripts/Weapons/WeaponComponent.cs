using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponComponent : ItemComponent
{
    protected WeaponObject weaponSO;

    protected Attack primaryAttack;
    protected Attack secondaryAttack;
    protected bool useAmmo = false;
    protected int currentAmmo;
    protected int maxAmmo;

    private void Awake()
    {
        weaponSO = (WeaponObject)itemObject;

        this.primaryAttack = weaponSO.primaryAttack;
        this.secondaryAttack = weaponSO.secondaryAttack;
        this.useAmmo = weaponSO.useAmmo;
        this.currentAmmo = weaponSO.currentAmmo;
        this.maxAmmo = weaponSO.maxAmmo;
    }

    protected abstract void PrimaryAttack();
    protected abstract void SecondaryAttack();
    protected abstract void SpawnAttack(GameObject attackObject, Vector3 attackPosition, float projectileSpeed);
    protected abstract void Reload();

}
 