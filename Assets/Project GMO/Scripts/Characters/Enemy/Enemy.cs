using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character, ICanBeDamage
{

    [SerializeField] private ItemObject drops;

    public override void ReceiveDamage(Damage damage)
    {
        Health -= damage.DamageAmount;

        Death();
    }

    private void Death()
    {
        if(Health <= 0)
        {
            GetComponent<Dispenser>().Dispense(drops);
            Destroy(gameObject);
        }
    }
}
