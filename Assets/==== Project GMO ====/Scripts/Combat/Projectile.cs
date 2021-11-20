using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Damage damage = null;

    private bool expent;

    public void SetDamage(Damage dmg)
    {
        damage = dmg;
    }

    private void Start()
    {
        print("HI");
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (other.isTrigger) return;

        print(other.gameObject);
        Destroy(gameObject);

        if (damage == null) return;

        if (other.GetComponentInParent<ICanBeDamage>() != null && !expent)
        {
            other.GetComponentInParent<ICanBeDamage>().ReceiveDamage(damage);
            expent = true;
        }
    }
}
