using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Station : MonoBehaviour, IHaveInfoName, IHealth, ICanBeAttack, ICanBeDamage
{
    [SerializeField] protected string stationName;
    public int StationPrice;

    [SerializeField] protected int health = 10;

    protected int currentHealth;
    protected int currentMaxHealth;

    public event ICanBeDamage.DamageCallback OnReceivedDamage;
    public event IHealth.HealthChangeCallback OnHealthChanged;

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
        OnHealthChanged?.Invoke(currentHealth, currentMaxHealth);
    }

    public int MaxHealth { get => currentMaxHealth; set => SetMaxHealth(value); }
    private void SetMaxHealth(int maxHealth)
    {
        currentMaxHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, currentMaxHealth);
    }

    public string InfoName { get => stationName; set => stationName = value; }

    public void ReceiveAttack()
    {
        
    }

    public virtual void ReceiveDamage(Damage damage)
    {
        currentHealth -= damage.DamageAmount;

        if(currentHealth <= 0)
        {
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}
