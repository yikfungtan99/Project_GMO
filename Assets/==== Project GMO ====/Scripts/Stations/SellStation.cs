using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SellStation : Station, ICanPickUpItems
{
    private MoneyManager moneyManager;

    private void Start()
    {
        moneyManager = GameObject.FindGameObjectWithTag("MoneyManager").GetComponent<MoneyManager>();
    }

    protected virtual void Sell(ItemData item)
    {
        moneyManager.SellItem(item);
    }

    public virtual void PickUpItem(Pickups pickup)
    {
        Destroy(pickup.gameObject);
    }
}
