using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private WeaponObject weaponSO;

    private bool useAmmo = false;
    private int currentAmmo;
    private int maxMagAmmo;
    private int maxAmmo;

    private Transform weaponFireLocation;

    private Attack primaryAttack;
    private Attack secondaryAttack;

    private void Awake()
    {
        this.primaryAttack = weaponSO.primaryAttack;
        this.secondaryAttack = weaponSO.secondaryAttack;
        this.useAmmo = weaponSO.useAmmo;
        this.maxMagAmmo = weaponSO.maxMagAmmo;
        this.maxAmmo = weaponSO.maxAmmo;
    }

    private float primaryAttackTime = 0;

    private void Start()
    {
        currentAmmo = maxMagAmmo;
        Transform weaponTransform = Instantiate(weaponSO.weaponGameObject, weaponHolder).transform;
        weaponFireLocation = weaponTransform.GetChild(0);
    }

    private void Update()
    {
        PrimaryAttack();
        Reload();
    }
    private void PrimaryAttack()
    {
        if (primaryAttack.fireMode == FireMode.AUTOMATIC)
        {
            if (Input.GetMouseButton(0))
            {
                if(currentAmmo > 0)
                {
                    if (primaryAttackTime > 0)
                    {
                        primaryAttackTime -= Time.deltaTime;
                    }
                    else
                    {
                        SpawnAttack(primaryAttack.projectile, weaponFireLocation.position, primaryAttack.projectileSpeed);
                        currentAmmo--;
                        primaryAttackTime = (primaryAttack.attackRate);
                        Reload();
                    }
                }
            }
        }

        if (primaryAttack.fireMode == FireMode.SINGLE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpawnAttack(primaryAttack.projectile, weaponFireLocation.position, primaryAttack.projectileSpeed);
                primaryAttackTime = 0;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            primaryAttackTime = 0;
        }
    }
    private void SpawnAttack(GameObject attackObject, Vector3 attackPosition, float projectileSpeed)
    {
        GameObject bulletInstance = Instantiate(attackObject, attackPosition, Quaternion.identity);
        bulletInstance.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        bulletInstance.GetComponent<Projectile>().SetDamage(primaryAttack.damage);
    }

    private void SecondaryAttack()
    {
        throw new System.NotImplementedException();
    }

    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (maxAmmo - maxMagAmmo >= 0)
            {
                currentAmmo = maxMagAmmo;
                maxAmmo -= maxMagAmmo;
            }
            else
            {
                print("Out of ammo");
            }
        }
    }
}
