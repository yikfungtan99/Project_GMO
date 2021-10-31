using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
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

    private float primaryAttackTime = 0;

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
        PrimaryAttack();
        Reload();
    }
    private void PrimaryAttack()
    {
        if (!isReloading)
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

    private async void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            float reloadWait = Time.time + reloadTime;

            while(Time.time < reloadWait)
            {
                isReloading = true;
                await System.Threading.Tasks.Task.Yield();
            }

            if (currentAmmo > 0)
            {
                int reloadAmount = currentAmmo < maxMagAmmo - currentMagAmmo ? currentAmmo : maxMagAmmo - currentMagAmmo;
                currentAmmo -= reloadAmount;
                currentMagAmmo += reloadAmount;

                OnWeaponInformationChange?.Invoke(currentMagAmmo, currentAmmo);
            }

            isReloading = false;
        }
    }

    public void GetAmmo(int amount)
    {
        currentAmmo += amount;
        OnWeaponInformationChange?.Invoke(currentMagAmmo, currentAmmo);
    }
}
