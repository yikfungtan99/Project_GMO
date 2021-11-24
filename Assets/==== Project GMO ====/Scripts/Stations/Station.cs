using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Station : MonoBehaviour, IHaveInfoName, IHealth
{
    [SerializeField] protected string stationName;
    public int StationPrice;

    [SerializeField] protected int health = 10;

    protected int currentHealth;
    protected int currentMaxHealth;

    public event ICanBeDamage.DamageCallback OnReceivedDamage;

    public delegate void DestroyCallback();
    public event DestroyCallback OnDestroyed;

    protected virtual void Awake()
    {
        currentHealth = health;
        currentMaxHealth = health;
    }

    public int Health { get => currentHealth; set => SetHealth(value); }
    private void SetHealth(int health)
    {
        currentHealth = health;
    }

    public int MaxHealth { get => currentMaxHealth; set => SetMaxHealth(value); }
    private void SetMaxHealth(int maxHealth)
    {
        currentMaxHealth = maxHealth;
    }

    public string InfoName { get => stationName; set => stationName = value; }

}
