using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Station : MonoBehaviour, IHaveInfoName, IHaveHealth, ICanBeAttack, ICanBeDamage
{
    [SerializeField] protected string stationName;
    [SerializeField] protected int health = 10;

    protected int currentHealth;
    protected int maxHealth;
    protected virtual void Awake()
    {
        currentHealth = health;
        maxHealth = health;
    }

    public int Health { get => currentHealth; set => currentHealth = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public string InfoName { get => stationName; set => stationName = value; }

    public void ReceiveAttack()
    {
        
    }

    public virtual void ReceiveDamage(Damage damage)
    {
        Health -= damage.DamageAmount;
    }
}
