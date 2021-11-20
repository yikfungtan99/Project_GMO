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
    [SerializeField] private WeaponObject[] weaponSO = new WeaponObject[2];

    [HideInInspector] public List<WeaponComponent> currentWeapons = new List<WeaponComponent>();

    public WeaponComponent currentWeapon;
    private int currentWeaponIndex = 0;

    public event Action<WeaponRestrictor> OnWeaponEquipped = delegate { };

    private void Start()
    {
        GainWeapon(weaponSO[0]);
    }
    private void Update()
    {
        Melee();
        WeaponPrimaryAttack();
        WeaponReload();
        //SwapWeapon();
    }

    private void Melee()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isMeleeing = true;
            GetComponent<Animator>().Play("Melee");
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

    private void GainWeapon(WeaponObject weaponSO)
    {
        GameObject weaponGOInstance = Instantiate(weaponSO.weaponGameObject, weaponHolder);
        WeaponComponent gainedWeapon = weaponGOInstance.GetComponent<WeaponComponent>();
        currentWeapons.Add(gainedWeapon);
        currentWeaponIndex += 1;
        EquipWeapon(gainedWeapon);
    }
    private void EquipWeapon(WeaponComponent weapon)
    {
        if(currentWeapon != null) currentWeapon.gameObject.SetActive(false);
        currentWeapon = weapon;
        currentWeapon.gameObject.SetActive(true);

        OnWeaponEquipped?.Invoke(weapon.GetComponent<WeaponRestrictor>());
    }
    private void SwapWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q) && currentWeapons.Count > 1)
        {
            currentWeaponIndex += 1;

            if (currentWeaponIndex > 1)
            {
                currentWeaponIndex = 0;
            }

            EquipWeapon(currentWeapons[currentWeaponIndex]);
        }
    }
    private void WeaponPrimaryAttack()
    {
        if (isMeleeing) return;
        if (Input.GetMouseButton(0))
        {
            currentWeapon.PrimaryFireInput();
        }
    }

    private void WeaponReload()
    {
        if (isMeleeing) return;

        WeaponRestrictor wr = currentWeapon.GetComponent<WeaponRestrictor>();

        if (wr != null)
        {
            wr.ReplenishWeapon();
        }
    }
    public void GainAmmo(int amount)
    {
        Ammo wr = currentWeapon.GetComponent<WeaponRestrictor>() as Ammo;

        if(wr != null)
        {
            wr.GainAmmo(amount);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if(meleePos != null) Gizmos.DrawWireSphere(meleePos.position, meleeRadius);
    }
}
