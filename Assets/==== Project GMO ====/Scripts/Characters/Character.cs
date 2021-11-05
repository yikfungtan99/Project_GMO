using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveInfoName
{
    public string InfoName { get; set; }
}

public abstract class Character : MonoBehaviour, IHaveInfoName, IHaveHealth, ICanBeDamage
{
    [SerializeField] private string characterName;
    [SerializeField] private int health;
    [SerializeField] private int armor;
    [SerializeField] private float speed;

    private int currentHealth = 0;
    private int currentMaxHealth = 0;

    protected void Awake()
    {
        currentHealth = health;
        currentMaxHealth = health;
    }

    public string CharacterName { get => characterName; set => characterName = value; }
    public int Health { get => currentHealth; set => SetHealth(value); }

    private void SetHealth(int health)
    {

        currentHealth = health;

        if(currentHealth >= currentMaxHealth)
        {
            currentHealth = currentMaxHealth;
        }

        OnHealthChanged?.Invoke(currentHealth, currentMaxHealth);
    }

    public int MaxHealth { get => currentMaxHealth; set => SetMaxHealth(value); }
    private void SetMaxHealth(int maxHealth)
    {
        currentMaxHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, currentMaxHealth);
    }

    public int Armor { get => armor; set => armor = value; }
    public float Speed { get => speed; set => speed = value; }
    public string InfoName { get => characterName; set => characterName = value; }
    
    public abstract void ReceiveDamage(Damage damage);
    public virtual event ICanBeDamage.DamageCallback OnReceivedDamage;

    public event IHaveHealth.HealthChangeCallback OnHealthChanged;
}
