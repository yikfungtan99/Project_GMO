using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, ICanPickUpItems
{
    [SerializeField] private PlayerWeapon playerWeapon;

    [SerializeField] private List<ItemData> items = new List<ItemData>();
    [SerializeField] private int maxInventorySlot = 5;
    [SerializeField] private int currentInventorySlot = 3;

    [SerializeField] private Transform tossPosition;
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private float tossForce;

    [SerializeField] private int currentSelectionIndex = 0;
    private int prevSelectionIndex = 0;

    public delegate void PlayerInventoryItemChangesCallback(List<ItemData> items);
    public delegate void PlayerInventorySelectionChangesCallback(int selectionIndex, string name);
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
                OnSelectionChanges?.Invoke(currentSelectionIndex, items[currentSelectionIndex].itemName);
                prevSelectionIndex = currentSelectionIndex;
            }

        }
    }

    public ItemData GetSelectedItem()
    {
        if (currentSelectionIndex >= 0 && currentSelectionIndex < items.Count)
        {
            return items[currentSelectionIndex];
        }

        return null;
    }

    private ItemData GetSelectedItem(int index)
    {
        if(index >= 0 && index < items.Count)
        {
            return items[index];
        }

        return null;
    }

    public void RemoveSelectedItem()
    {
        ItemData selected = GetSelectedItem();
        if (selected != null) RemoveItem(selected);
    }

    public void GainItem(ItemData item)
    {
        if(items.Count < currentInventorySlot)
        {
            items.Add(item);
            OnInventoryChanges?.Invoke(items);
            OnSelectionChanges?.Invoke(currentSelectionIndex, items[currentSelectionIndex].itemName);
        }
    }

    public void TossItem()
    {
        ItemData selected = GetSelectedItem(currentSelectionIndex);
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

        OnSelectionChanges?.Invoke(currentSelectionIndex, items.Count != 0 ? items[currentSelectionIndex].itemName : "");
    }

    public void PickUpItem(Pickups pickup)
    {
        if (items.Count < currentInventorySlot)
        {
            GainItem(pickup.GetPickupItem());
            Destroy(pickup.gameObject);

            if (items.Count == 1)
            {
                GetSelectedItem(currentSelectionIndex);
            }
        }
    }
}
