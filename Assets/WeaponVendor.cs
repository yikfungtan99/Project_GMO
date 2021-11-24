using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponVendor : MonoBehaviour
{
    public List<string> weaponsName = new List<string>();
    public List<GameObject> weapons = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI weaponName;

    private GameObject currentWeapon;

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
        weaponName.text = weaponsName[currentSelectionIndex];
    }

    public void GiveWeapon()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().GainWeapon(currentWeapon);
    }
}
