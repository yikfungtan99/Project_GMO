using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, ICanPickUpItems
{
    [SerializeField] private List<ItemData> items = new List<ItemData>();
    [SerializeField] private int maxInventorySlot = 5;
    [SerializeField] private int currentInventorySlot = 3;

    [SerializeField] private Transform tossPosition;
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private float tossForce;

    [SerializeField] private int currentSelectionIndex = 0;
    private int prevSelectionIndex = 0;

    public delegate void PlayerInventoryItemChangesCallback(List<ItemData> items);
    public delegate void PlayerInventorySelectionChangesCallback(int selectionIndex);
    public delegate void PlayerInventoryItemSlotChangesCallback();

    public event PlayerInventoryItemChangesCallback OnInventoryChanges;
    public event PlayerInventorySelectionChangesCallback OnSelectionChanges;
    public event PlayerInventoryItemSlotChangesCallback OnGainItemSlot;

    private void Start()
    {
        for (int i = 0; i < currentInventorySlot; i++)
        {
            GainInventorySlot();
        }
    }

    private void Update()
    {
        SwitchSelection();

        if (Input.GetKeyDown(KeyCode.G))
        {
            TossItem();
        }
    }

    private void GainInventorySlot()
    {
        OnGainItemSlot?.Invoke();
    }

    private void SwitchSelection()
    {
        if (items.Count > 0)
        {
            currentSelectionIndex += (int)Input.mouseScrollDelta.y * -1;

            if (currentSelectionIndex < 0)
            {
                currentSelectionIndex = items.Count - 1;
            }

            if (currentSelectionIndex >= items.Count)
            {
                currentSelectionIndex = 0;
            }

            if (prevSelectionIndex != currentSelectionIndex)
            {
                OnSelectionChanges?.Invoke(currentSelectionIndex);
                prevSelectionIndex = currentSelectionIndex;
            }

        }
    }

    private ItemData SelectItem(int index)
    {
        if(index >= 0 && index < items.Count)
        {
            return items[index];
        }

        return null;
    }

    public void GetItem(ItemData item)
    {
        if(items.Count < currentInventorySlot)
        {
            items.Add(item);
            OnInventoryChanges?.Invoke(items);
        }
    }

    public void TossItem()
    {
        ItemData selected = SelectItem(currentSelectionIndex);
        if (selected == null) return;

        GameObject droppedItem = Instantiate(pickupPrefab, tossPosition.position, Quaternion.identity);
        droppedItem.GetComponent<Pickups>().SetPickupItem(selected);
        droppedItem.GetComponent<Rigidbody>().AddForce(tossPosition.forward * tossForce);

        RemoveItem(selected);
    }

    private void RemoveItem(ItemData selected)
    {
        items.Remove(selected);

        OnInventoryChanges?.Invoke(items);

        if (currentSelectionIndex > 0)
        {
            currentSelectionIndex -= 1;
        }
    }

    public void PickUpItem(Pickups pickup)
    {
        if (items.Count < currentInventorySlot)
        {
            GetItem(pickup.GetPickupItem());
            Destroy(pickup.gameObject);

            if (items.Count == 1)
            {
                SelectItem(currentSelectionIndex);
                OnSelectionChanges?.Invoke(currentSelectionIndex);
            }
        }
    }
}
