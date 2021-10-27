using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour, ICanBeAttack, ICanBeDamage
{
    [SerializeField] private int health = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ReceiveAttack()
    {
        
    }

    public void ReceiveDamage(Damage damage)
    {
        health -= damage.DamageAmount;
    }
}
