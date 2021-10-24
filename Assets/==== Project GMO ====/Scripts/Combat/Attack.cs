using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum FireMode
{
    AUTOMATIC,
    SINGLE,
}

[System.Serializable]
public class AttackType
{

}

[System.Serializable]
public class AttackEffect
{

}


[System.Serializable]
public class Attack
{
    public Damage damage;
    public float attackRange;
    public float attackRate;
    public LayerMask attackLayer;
    public AttackType attackType;
    public AttackEffect attackEffect;
    public FireMode fireMode;

    public GameObject projectile;
    public float projectileSpeed;
}
