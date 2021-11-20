using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponVendor : MonoBehaviour
{
    [SerializeField] private List<WeaponObject> weapons = new List<WeaponObject>();
    [SerializeField] private TextMeshProUGUI weaponName;

    private WeaponObject currentWeapon;

    private int currentSelectionIndex = 0;

    private void Start()
    {
        currentWeapon = weapons[currentSelectionIndex];
        UpdateInterface();
    }

    public void NextWeapon()
    {
        currentSelectionIndex++;

        if (currentSelectionIndex >= weapons.Count) currentSelectionIndex = 0;

        currentWeapon = weapons[currentSelectionIndex];

        UpdateInterface();
    }

    public void PrevWeapon()
    {
        currentSelectionIndex--;

        if (currentSelectionIndex < 0) currentSelectionIndex = weapons.Count - 1;

        currentWeapon = weapons[currentSelectionIndex];

        UpdateInterface();
    }

    private void UpdateInterface()
    {
        weaponName.text = currentWeapon.itemName;
    }

    public void GiveWeapon()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().GainWeapon(currentWeapon);
    }
}
