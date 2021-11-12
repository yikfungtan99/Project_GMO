using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveInfoName
{
    public string InfoName { get; set; }
}

public abstract class Character : MonoBehaviour, IHaveInfoName, IBuffableHealth, ICanBeDamage
{
    [SerializeField] private string characterName;
    [SerializeField] private int health;
    [SerializeField] private float speed;

    private int currentHealth = 0;
    private int currentMaxHealth = 0;
    private int trueMaxHealth = 0;
    private int additionalMaxHealth = 0;

    protected void Awake()
    {
        currentHealth = health;
        currentMaxHealth = health;
        trueMaxHealth = health;
    }

    public string CharacterName { get => characterName; set => characterName = value; }
    public int Health { get => currentHealth; set => SetHealth(value); }

    protected virtual void SetHealth(int health)
    {
        currentHealth = health;

        if(currentHealth >= currentMaxHealth)
        {
            currentHealth = currentMaxHealth;
        }
    }

    public int MaxHealth { get => currentMaxHealth; set => SetMaxHealth(value); }
    public int BaseMaxHealth { get => trueMaxHealth; set => SetTrueMaxHealth(value); }
    public int AdditionalMaxHealth { get => additionalMaxHealth; set => SetAdditionalMaxHealth(value); }
    protected virtual void SetMaxHealth(int maxHealth)
    {
        currentMaxHealth = maxHealth;
        SetHealth(currentHealth);
    }

    protected virtual void SetTrueMaxHealth(int tMaxHealth)
    {
        trueMaxHealth = tMaxHealth;
        SetMaxHealth(trueMaxHealth + additionalMaxHealth);
    }

    protected virtual void SetAdditionalMaxHealth(int addMaxHealth)
    {
        additionalMaxHealth = addMaxHealth;
        SetMaxHealth(trueMaxHealth + additionalMaxHealth);
    }

    public float Speed { get => speed; set => speed = value; }
    public string InfoName { get => characterName; set => characterName = value; }
    public abstract void ReceiveDamage(Damage damage);
    public virtual event ICanBeDamage.DamageCallback OnReceivedDamage;
}
