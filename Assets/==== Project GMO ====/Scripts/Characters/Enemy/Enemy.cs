using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character, ICanBeDamage
{
    public delegate void DeathCallback(Enemy enemy = null);
    public DeathCallback OnDeath;

    public override void ReceiveDamage(Damage damage)
    {
        print(damage.DamageAmount);
        Health -= damage.DamageAmount;

        Death();
    }

    private void Death()
    {
        if(Health <= 0)
        {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
