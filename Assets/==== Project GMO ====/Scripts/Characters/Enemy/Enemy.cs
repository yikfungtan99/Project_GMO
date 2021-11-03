using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character, ICanBeDamage
{
    [SerializeField] private ItemObject drops;

    private GameDirector director;

    public delegate void DeathCallback(Enemy enemy = null);
    public DeathCallback OnDeath;

    public override void ReceiveDamage(Damage damage)
    {
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

    public void SetDirector(GameDirector dir)
    {
        director = dir;
    }
}
