using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleAttackCollider : MonoBehaviour
{
    private Damage attackDamage;
    private float meleeAttackRadius;
    private LayerMask attackLayer;

    public void SetUpAttack(Damage attackDamage, float meleeAttackRadius, LayerMask attackLayer)
    {
        this.attackDamage = attackDamage;
        this.meleeAttackRadius = meleeAttackRadius;
        this.attackLayer = attackLayer;
    }

    private void Attack()
    {
        Collider[] hitTargets = Physics.OverlapSphere(transform.position, meleeAttackRadius);

        for (int i = 0; i < hitTargets.Length; i++)
        {
            ICanBeDamage canBeDamage = hitTargets[i].GetComponentInParent<ICanBeDamage>();
            if(canBeDamage != null) canBeDamage.ReceiveDamage(attackDamage);

            print("HI");
        }
    }
}
