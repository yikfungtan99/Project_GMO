using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
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

    private void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();
        playerInventory = playerObject.GetComponent<PlayerInventory>();
        playerWeapon = playerObject.GetComponent<PlayerCombat>();

        player.OnHealthChanged += UpdateHealth;

        playerInventory.OnInventoryChanges += HolsterUpdateInventory;
        playerInventory.OnSelectionChanges += HolsterUpdateSelected;
        playerInventory.OnGainItemSlot += GainInventorySlot;

        playerWeapon.OnWeaponEquipped += WeaponEquipped;
    }

    private void UpdateHealth(int curHealth, int curMaxHealth, int addMaxHealth)
    {
        currentHealthText.text = curHealth.ToString();
        currentMaxHealthText.text = curMaxHealth.ToString();

        string addMaxHealthString;
        Color addMaxHealthTextColor = Color.white;

        if(addMaxHealth > 0)
        {
            addMaxHealthString = "+" + addMaxHealth;
            addMaxHealthTextColor = Color.green;
        }
        else if(addMaxHealth == 0)
        {
            addMaxHealthString = string.Empty;
        }
        else
        {
            addMaxHealthString = addMaxHealth.ToString();
            addMaxHealthTextColor = Color.red;
        }

        additionalMaxHealthText.text = addMaxHealthString;
        additionalMaxHealthText.color = addMaxHealthTextColor;
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

    private void WeaponEquipped(WeaponRestrictor wr)
    {
        if (wr == null) return;

        Ammo ammo = wr as Ammo;

        if(ammo != null)
        {
            ammo.OnMagChanged += WeaponUpdateMag;
            ammo.OnAmmoChanged += WeaponUpdateAmmo;
        }
    }

    private void WeaponUpdateMag(int mag)
    {
        weaponMagText.text = mag.ToString();
    }

    private void WeaponUpdateAmmo(int ammo)
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
    }
}
