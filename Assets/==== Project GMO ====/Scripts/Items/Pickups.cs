using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private Transform pickupModel;

    [SerializeField] private float baseHeight;
    [SerializeField] private float speed;
    [SerializeField] private float amp;

    [SerializeField] private SpriteRenderer itemImageRenderer;
    [SerializeField] private float itemImageRotateSpeed;

    float sinStep = 0;

    private void Start()
    {
        itemImageRenderer.sprite = itemData.itemObject.itemImage;
    }

    private void Update()
    {
        sinStep += 1 * Time.deltaTime;

        pickupModel.localPosition = new Vector3(pickupModel.localPosition.x, baseHeight + (amp * Mathf.Sin(sinStep * speed)), pickupModel.localPosition.z);

        if(sinStep > 999999)
        {
            sinStep = 0;
        }

        itemImageRenderer.transform.Rotate(0, itemImageRotateSpeed * Time.deltaTime, 0);
    }

    public void SetPickupItem(ItemData setItem)
    {
        itemData = setItem;
    }

    public ItemData GetPickupItem()
    {
        return itemData;
    }

    private void OnTriggerStay(Collider other)
    {
        PickedUp(other);
    }

    private void PickedUp(Collider other)
    {
        if (other.transform.GetComponentInParent<ICanPickUpItems>() != null)
        {
            other.transform.GetComponentInParent<ICanPickUpItems>().PickUpItem(this);
        }
    }
}
