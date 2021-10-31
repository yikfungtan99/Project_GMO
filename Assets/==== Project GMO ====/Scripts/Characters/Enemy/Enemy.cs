using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEventArgs : EventArgs
{
    public bool isLastEnemy = false;
}

public class Enemy : Character, ICanBeDamage
{
    [SerializeField] private ItemObject drops;

    private GameDirector director;

    public event EventHandler OnDeath;

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
            DeathEvent(new DeathEventArgs {});
            Destroy(gameObject);
        }
    }

    private void DeathEvent(DeathEventArgs e)
    {
        OnDeath?.Invoke(this, e);
    }

    public void SetDirector(GameDirector dir)
    {
        director = dir;
    }
}
