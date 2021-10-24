using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private Transform dispensePosition;
    [SerializeField] private float dispenserForceVariationMin = 100f;
    [SerializeField] private float dispenserForceVariationMax;

    public dynamic CreateItem(ItemObject item)
    {
        IngredientObject castIngredient = item as IngredientObject;

        if (castIngredient != null)
        {
            return new IngredientData(castIngredient);
        }

        DishObject castDish = item as DishObject;

        if (castDish != null)
        {
            return new DishData(castDish);
        }

        return new ItemData(item);
    }

    public void Dispense(ItemObject item, int amount = 1)
    {
        if (item == null) return;

        GameObject droppedItem = Instantiate(pickupPrefab, dispensePosition.position, Quaternion.identity);
        float force = Random.Range(dispenserForceVariationMin, dispenserForceVariationMax);

        droppedItem.GetComponent<Pickups>().SetPickupItem(CreateItem(item));
        droppedItem.GetComponent<Rigidbody>().AddForce(dispensePosition.forward * force);
    }
}
