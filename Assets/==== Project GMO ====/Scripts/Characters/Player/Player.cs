using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public override event ICanBeDamage.DamageCallback OnReceivedDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ReceiveDamage(Damage damage)
    {
        //Shake Camera
        Camera.main.GetComponent<CameraShake>().ShakeCamera(damage.DamageAmount, 0.25f);
        OnReceivedDamage?.Invoke(damage.DamageAmount);
        Health -= damage.DamageAmount;

        Death();
    }

    private void Death()
    {
        if(Health <= 0)
        {
            Health = 0;
        }
    }
}
