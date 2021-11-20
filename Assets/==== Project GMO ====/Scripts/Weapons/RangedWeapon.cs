using System;
using UnityEngine;

public class RangedWeapon : WeaponComponent
{
    public override void PrimaryFire()
    {
        SpawnAttack(primaryAttack.projectile, weaponFireLocation, primaryAttack.projectileSpeed);
        if(weaponRestrictor != null) weaponRestrictor.ExhaustWeapon();
    }

    protected override void SpawnAttack(GameObject attackObject, Transform fireLocation, float projectileSpeed)
    {
        GameObject bulletInstance = Instantiate(attackObject, fireLocation.position, fireLocation.rotation);
        bulletInstance.GetComponent<Rigidbody>().AddForce(fireLocation.forward * projectileSpeed);
        bulletInstance.GetComponent<Projectile>().SetDamage(primaryAttack.damage);
    }

    public override void SecondaryAttack()
    {
        throw new System.NotImplementedException();
    }
}
