using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform meleePos;
    [SerializeField] private float meleeRadius;
    [SerializeField] private int meleeDamage;

    private bool isMeleeing = false;

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private WeaponObject weaponSO;

    public WeaponComponent equippedWeapon;

    public delegate void WeaponEquippedCallback(WeaponComponent weapon);
    public event WeaponEquippedCallback OnWeaponEquipped;

    private void Start()
    {
        EquipWeapon(weaponSO.weaponGameObject);
    }
    private void Update()
    {
        Melee();
        WeaponPrimaryAttack();
        WeaponReload();
    }
    private void EquipWeapon(GameObject weaponGameObject)
    {
        GameObject weaponGOInstance = Instantiate(weaponGameObject, weaponHolder);
        WeaponComponent weapon = weaponGOInstance.GetComponent<WeaponComponent>();

        equippedWeapon = weapon;

        OnWeaponEquipped?.Invoke(weapon);
    }
    private void Melee()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            bool canMelee = !equippedWeapon.isReloading;

            if (canMelee)
            {
                isMeleeing = true;
                GetComponent<Animator>().Play("Melee");
            }
        }
    }

    private void MeleeExecution()
    {
        Collider[] hits = Physics.OverlapSphere(meleePos.position, meleeRadius);
        HashSet<ICanBeDamage> hitted = new HashSet<ICanBeDamage>();

        for (int i = 0; i < hits.Length; i++)
        {
            ICanBeDamage canBeDamage = hits[i].GetComponentInParent<ICanBeDamage>();

            if (canBeDamage != null)
            {
                if (hits[i].transform.parent != transform)
                {
                    if (!hitted.Contains(canBeDamage))
                    {
                        print(canBeDamage);
                        canBeDamage.ReceiveDamage(new Damage(meleeDamage, null));
                        hitted.Add(canBeDamage);
                    }
                }
            }
        }
    }

    private void MeleeEnd()
    {
        isMeleeing = false;
    }

    private void WeaponPrimaryAttack()
    {
        if (Input.GetMouseButton(0))
        {
            equippedWeapon.PrimaryFireInput();
        }
    }

    private void WeaponReload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            equippedWeapon.Reload();
        }
    }

    public void GainAmmo(int amount)
    {
        equippedWeapon.GainAmmo(amount);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if(meleePos != null) Gizmos.DrawWireSphere(meleePos.position, meleeRadius);
    }
}
