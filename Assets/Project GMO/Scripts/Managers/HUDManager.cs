using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Transform holsterPanel;

    [SerializeField] private GameObject itemSlotPrefab;
    private Image prevImage;

    private void OnEnable()
    {
        playerInventory.OnInventoryChanges += HolsterUpdateInventory;
        playerInventory.OnSelectionChanges += HolsterUpdateSelected;
        playerInventory.OnGainItemSlot += GainInventorySlot;
    }

    private void HolsterUpdateSelected(int selectedIndex)
    {
        if (prevImage != null) prevImage.color = new Color(1, 1, 1, 0.25f);
        Image slotImage = holsterPanel.GetChild(selectedIndex).GetComponentInChildren<Image>();

        slotImage.color = Color.yellow;
        prevImage = slotImage;
    }

    private void HolsterUpdateInventory(List<ItemData> items)
    {
        for (int i = 0; i < holsterPanel.childCount; i++)
        {
            Image slotImage = holsterPanel.GetChild(i).GetChild(0).GetComponentInChildren<Image>();

            if(i < items.Count)
            {
                slotImage.enabled = true;
                slotImage.sprite = items[i].itemObject.itemImage;
            }
            else
            {
                slotImage.enabled = false;
                slotImage.sprite = null;
            }
            
        }
    }

    private void GainInventorySlot()
    {
        Instantiate(itemSlotPrefab, holsterPanel);
    }

    private void OnDisable()
    {
        playerInventory.OnInventoryChanges -= HolsterUpdateInventory;
        playerInventory.OnSelectionChanges -= HolsterUpdateSelected;
        playerInventory.OnGainItemSlot -= GainInventorySlot;
    }
}
