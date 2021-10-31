using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishPortal : SellStation
{
    public override void PickUpItem(Pickups pickup)
    {
        DishData dish = pickup.GetPickupItem() as DishData;

        if (dish != null)
        {
            Sell(dish);
            base.PickUpItem(pickup);
        }
    }
}
