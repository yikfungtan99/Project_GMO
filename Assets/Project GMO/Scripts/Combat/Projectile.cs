using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Damage damage = null;

    public void SetDamage(Damage dmg)
    {
        damage = dmg;
    }

    private void Start()
    {
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        if (damage == null) return;

        if (other.GetComponentInParent<ICanBeDamage>() != null)
        {
            other.GetComponentInParent<ICanBeDamage>().ReceiveDamage(damage);
        }
    }
}
