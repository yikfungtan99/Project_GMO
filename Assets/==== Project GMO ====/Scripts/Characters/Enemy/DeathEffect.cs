using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public abstract class DeathEffect : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Enemy>().OnDeath += Effect;
    }

    protected abstract void Effect(Enemy enemy);
}