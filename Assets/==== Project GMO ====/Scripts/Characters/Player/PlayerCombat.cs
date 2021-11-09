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

    private bool useAmmo = false;
    private int currentMagAmmo;
    private int maxMagAmmo;
    private int currentAmmo;
    private int maxAmmo;

    private float reloadTime;
    private bool isReloading;

    private Transform weaponFireLocation;

    private Attack primaryAttack;
    private Attack secondaryAttack;

    private float primaryAttackTime = 0;

    public delegate void WeaponInformationChangeCallback(int mag, int ammo);
    public event WeaponInformationChangeCallback OnWeaponInformationChange;

    private void Awake()
    {
        this.primaryAttack = weaponSO.primaryAttack;
        this.secondaryAttack = weaponSO.secondaryAttack;
        this.useAmmo = weaponSO.useAmmo;
        this.maxMagAmmo = weaponSO.maxMagAmmo;
        this.maxAmmo = weaponSO.maxAmmo;
        this.reloadTime = weaponSO.reloadTime;
    }

    private void Start()
    {
        currentMagAmmo = maxMagAmmo;
        currentAmmo = maxAmmo;
        Transform weaponTransform = Instantiate(weaponSO.weaponGameObject, weaponHolder).transform;
        weaponFireLocation = weaponTransform.GetChild(0);

        OnWeaponInformationChange?.Invoke(currentMagAmmo, currentAmmo);
    }

    private void Update()
    {
        Melee();
        PrimaryAttack();
        Reload();
    }

    private void Melee()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            bool canMelee = !isReloading;

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

    private void PrimaryAttack()
    {
        bool canFire = !isReloading && !isMeleeing;

        if (canFire)
        {
            if (primaryAttack.fireMode == FireMode.AUTOMATIC)
            {
                if (Input.GetMouseButton(0))
                {
                    if (currentMagAmmo > 0)
                    {
                        if (primaryAttackTime > 0)
                        {
                            primaryAttackTime -= Time.deltaTime;
                        }
                        else
                        {
                            SpawnAttack(primaryAttack.projectile, weaponFireLocation, primaryAttack.projectileSpeed);
                            currentMagAmmo--;
                            primaryAttackTime = (primaryAttack.attackRate);

                            OnWeaponInformationChange?.Invoke(currentMagAmmo, currentAmmo);

                            Reload();
                        }
                    }
                }
            }

            if (primaryAttack.fireMode == FireMode.SINGLE)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    SpawnAttack(primaryAttack.projectile, weaponFireLocation, primaryAttack.projectileSpeed);
                    primaryAttackTime = 0;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            primaryAttackTime = 0;
        }
    }

    private void SecondaryAttack()
    {
        throw new System.NotImplementedException();
    }

    private void SpawnAttack(GameObject attackObject, Transform attackPosition, float projectileSpeed)
    {
        GameObject bulletInstance = Instantiate(attackObject, attackPosition.position, attackPosition.rotation);
        bulletInstance.GetComponent<Rigidbody>().AddForce(attackPosition.forward * projectileSpeed);
        bulletInstance.GetComponent<Projectile>().SetDamage(primaryAttack.damage);
    }

    private void Reload()
    {
        bool canReload = !isReloading && currentMagAmmo < maxMagAmmo && currentAmmo > 0 && !isMeleeing;

        if (Input.GetKeyDown(KeyCode.R) && canReload)
        {
            isReloading = true;
            GetComponent<Animator>().Play("Reload");
        }
    }

    public void FillWeaponMagazine()
    {
        int amountToReload = ReloadAmount();
        currentMagAmmo += amountToReload;
        currentAmmo -= amountToReload;

        isReloading = false;

        OnWeaponInformationChange?.Invoke(currentMagAmmo, currentAmmo);
    }

    private int ReloadAmount()
    {
        int countToFullMag = maxMagAmmo - currentMagAmmo;

        return currentAmmo < countToFullMag ? currentAmmo : countToFullMag;
    }

    public void GainAmmo(int amount)
    {
        currentAmmo += amount;
        OnWeaponInformationChange?.Invoke(currentMagAmmo, currentAmmo);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if(meleePos != null) Gizmos.DrawWireSphere(meleePos.position, meleeRadius);
    }
}
