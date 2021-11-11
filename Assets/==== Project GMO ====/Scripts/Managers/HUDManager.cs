using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI currentMaxHealthText;
    [SerializeField] private TextMeshProUGUI additionalMaxHealthText;

    private PlayerInventory playerInventory;
    private PlayerCombat playerWeapon;

    [SerializeField] private Transform holsterPanel;

    [SerializeField] private TextMeshProUGUI weaponMagText;
    [SerializeField] private TextMeshProUGUI weaponAmmoText;

    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private TextMeshProUGUI selectedItemText;
    private Image prevImage;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();
        playerInventory = playerObject.GetComponent<PlayerInventory>();
        playerWeapon = playerObject.GetComponent<PlayerCombat>();

        player.OnHealthChanged += UpdateHealth;

        playerInventory.OnInventoryChanges += HolsterUpdateInventory;
        playerInventory.OnSelectionChanges += HolsterUpdateSelected;
        playerInventory.OnGainItemSlot += GainInventorySlot;

        playerWeapon.OnWeaponInformationChange += WeaponUpdateMagAmmo;
        playerWeapon.OnWeaponInformationChange += WeaponUpdateAmmo;
    }

    private void UpdateHealth(int health, int maxHealth)
    {
        currentHealthText.text = health.ToString();
        currentMaxHealthText.text = maxHealth.ToString();
    }

    private void HolsterUpdateSelected(int selectedIndex, string itemName)
    {
        if (prevImage != null) prevImage.color = new Color(1, 1, 1, 0.25f);
        Image slotImage = holsterPanel.GetChild(selectedIndex).GetComponentInChildren<Image>();

        slotImage.color = Color.yellow;
        prevImage = slotImage;

        selectedItemText.text = itemName;
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
    private void WeaponUpdateAmmo(int mag, int ammo)
    {
        weaponMagText.text = mag.ToString();
    }

    private void WeaponUpdateMagAmmo(int mag, int ammo)
    {
        weaponAmmoText.text = ammo.ToString();
    }

    private void GainInventorySlot()
    {
        GameObject slotInstance = Instantiate(itemSlotPrefab, holsterPanel);

        if(holsterPanel.childCount == 1)
        {
            slotInstance.GetComponent<Image>().color = Color.yellow;
        }
    }

    private void OnDisable()
    {
        playerInventory.OnInventoryChanges -= HolsterUpdateInventory;
        playerInventory.OnSelectionChanges -= HolsterUpdateSelected;
        playerInventory.OnGainItemSlot -= GainInventorySlot;

        playerWeapon.OnWeaponInformationChange -= WeaponUpdateMagAmmo;
        playerWeapon.OnWeaponInformationChange -= WeaponUpdateAmmo;
    }
}
