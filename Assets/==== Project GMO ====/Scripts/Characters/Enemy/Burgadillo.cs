using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burgadillo : Enemy
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Objective"))
        {
            ReceiveDamage(new Damage(Health, null));
        }
    }
}
