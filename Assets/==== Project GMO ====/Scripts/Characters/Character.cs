using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, ICanBeDamage
{
    [SerializeField] private string characterName;
    [SerializeField] private int health;
    [SerializeField] private int armor;
    [SerializeField] private float speed;

    public string CharacterName { get => characterName; set => characterName = value; }
    public int Health { get => health; set => health = value; }
    public int Armor { get => armor; set => armor = value; }
    public float Speed { get => speed; set => speed = value; }

    public abstract void ReceiveDamage(Damage damage);
}
