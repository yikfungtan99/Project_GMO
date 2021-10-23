using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeRangedWeapon : WeaponComponent
{
    [SerializeField] private Transform weaponFireLocation;

    private float primaryAttackTime = 0;

    private void Update()
    {
        PrimaryAttack();
    }

    protected override void PrimaryAttack()
    {
        if (primaryAttack.fireMode == FireMode.AUTOMATIC)
        {
            if (Input.GetMouseButton(0))
            {
                if (primaryAttackTime > 0)
                {
                    primaryAttackTime -= Time.deltaTime;
                }
                else
                {
                    SpawnAttack(primaryAttack.projectile, weaponFireLocation.position, primaryAttack.projectileSpeed);
                    primaryAttackTime = (primaryAttack.attackRate);
                }
            }
        }

        if (primaryAttack.fireMode == FireMode.SINGLE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpawnAttack(primaryAttack.projectile, weaponFireLocation.position, primaryAttack.projectileSpeed);
                primaryAttackTime = 0;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            primaryAttackTime = 0;
        }
    }
    protected override void SpawnAttack(GameObject attackObject, Vector3 attackPosition, float projectileSpeed)
    {
        GameObject bulletInstance = Instantiate(attackObject, attackPosition, Quaternion.identity);
        bulletInstance.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        bulletInstance.GetComponent<Projectile>().SetDamage(primaryAttack.damage);
    }

    protected override void SecondaryAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override void Reload()
    {
        throw new System.NotImplementedException();
    }
}
