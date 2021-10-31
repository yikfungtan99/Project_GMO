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
    private int maxHealth = 0;
    protected void Awake()
    {
        currentHealth = health;
        maxHealth = health;
    }

    public string CharacterName { get => characterName; set => characterName = value; }
    public int Health { get => currentHealth; set => currentHealth = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int Armor { get => armor; set => armor = value; }
    public float Speed { get => speed; set => speed = value; }
    public string InfoName { get => characterName; set => characterName = value; }
    

    public abstract void ReceiveDamage(Damage damage);
}
