using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public delegate void HealthChangeCallback(int curHealth, int curMaxHealth, int addMaxHealth);
    public event HealthChangeCallback OnHealthChanged;

    public override event ICanBeDamage.DamageCallback OnReceivedDamage;

    protected override void SetHealth(int health)
    {
        base.SetHealth(health);
        OnHealthChanged?.Invoke(Health, MaxHealth, AdditionalMaxHealth);
    }

    public override void ReceiveDamage(Damage damage)
    {
        //Shake Camera
        Camera.main.GetComponent<CameraShake>().ShakeCamera(damage.DamageAmount, 0.25f);
        OnReceivedDamage?.Invoke(damage.DamageAmount);
        Health -= damage.DamageAmount;

        print(Health);

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
