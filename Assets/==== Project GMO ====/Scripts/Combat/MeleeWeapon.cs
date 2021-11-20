using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponComponent
{
    [SerializeField] private float weaponHitRadius;
    public override void PrimaryFire()
    {
        Collider[] hits = Physics.OverlapSphere(weaponFireLocation.position, weaponHitRadius);
        HashSet<ICanBeDamage> hitted = new HashSet<ICanBeDamage>();

        for (int i = 0; i < hits.Length; i++)
        {
            ICanBeDamage canBeDamage = hits[i].GetComponentInParent<ICanBeDamage>();

            if (canBeDamage != null)
            {
                if (hits[i].transform.root != transform.root)
                {
                    if (!hitted.Contains(canBeDamage))
                    {
                        print(canBeDamage);
                        canBeDamage.ReceiveDamage(primaryAttack.damage);
                        hitted.Add(canBeDamage);
                    }
                }
            }
        }
    }

    public override void SecondaryAttack()
    {
        throw new NotImplementedException();
    }

    protected override void SpawnAttack(GameObject attackObject, Transform attackPosition, float projectileSpeed)
    {
        GameObject melee = Instantiate(attackObject, attackPosition.position, attackPosition.rotation);
        melee.GetComponent<MeeleAttackCollider>().SetUpAttack(primaryAttack.damage, weaponHitRadius, primaryAttack.attackLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponFireLocation.position, weaponHitRadius);
    }
}
