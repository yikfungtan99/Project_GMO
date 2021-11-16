using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeRangedWeapon : WeaponComponent
{
    public override void PrimaryFireInput()
    {
        if (canFire())
        {
            anim.speed = 1 / primaryAttack.attackRate;
            anim.Play("PrimaryFire");
        }
    }

    public override void PrimaryFire()
    {
        SpawnAttack(primaryAttack.projectile, weaponFireLocation, primaryAttack.projectileSpeed);
        ConsumeAmmo();
    }
    protected override void SpawnAttack(GameObject attackObject, Transform fireLocation, float projectileSpeed)
    {
        GameObject bulletInstance = Instantiate(attackObject, fireLocation.position, fireLocation.rotation);
        bulletInstance.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        bulletInstance.GetComponent<Projectile>().SetDamage(primaryAttack.damage);
    }
    protected override void ConsumeAmmo()
    {
        if (currentMagAmmo > 0)
        {
            currentMagAmmo--;
        }
        else
        {
            currentMagAmmo = 0;
        }

        UpdateAmmoInfo();
    }

    public override void SecondaryAttack()
    {
        throw new System.NotImplementedException();
    }
}
