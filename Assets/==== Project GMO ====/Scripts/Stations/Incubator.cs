using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedIncubate : EventArgs
{

}

public class Incubator : MonoBehaviour, ICanPickUpItems, ICanBeDamage
{
    [SerializeField] private int health;
    [SerializeField] private EggsenceData egg;
    [SerializeField] private float incubationTime;
    [SerializeField] private Transform incubateSpawnPosition;

    private bool isIncubating = false;

    public event EventHandler OnFinishedIncubate;

    private async void WaitForIncubation()
    {
        float waitTime = Time.time + incubationTime;


        while(Time.time < waitTime)
        {
            isIncubating = true;
            await System.Threading.Tasks.Task.Yield();
        }

        Instantiate(egg.foodling, incubateSpawnPosition.position, incubateSpawnPosition.rotation);
        isIncubating = false;
        IncubateEvent(new FinishedIncubate { });
    }

    private void IncubateEvent(FinishedIncubate e)
    {
        OnFinishedIncubate?.Invoke(this, e);
    }

    public void PickUpItem(Pickups pickup)
    {
        if (pickup.GetPickupItem().GetType() == typeof(EggsenceData) && !isIncubating)
        {
            egg = pickup.GetPickupItem() as EggsenceData;
            WaitForIncubation();
            Destroy(pickup.gameObject);
        }
    }

    public void ReceiveDamage(Damage damage)
    {
        health -= damage.DamageAmount;

        if(health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        print("Game Over");
    }
}
