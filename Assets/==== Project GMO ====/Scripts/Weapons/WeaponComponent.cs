using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class WeaponComponent : ItemComponent
{
    [SerializeField] protected Transform weaponFireLocation;
    [SerializeField] protected Animator anim;
    protected WeaponObject weaponSO;

    protected Attack primaryAttack;
    protected Attack secondaryAttack;
    protected bool useAmmo = false;
    public bool isReloading = false;

    protected int currentMagAmmo;
    protected int currentMagMaxAmmo;

    protected int currentAmmo;
    protected int maxAmmo;

    public delegate void WeaponInformationChangeCallback(int mag, int ammo);
    public event WeaponInformationChangeCallback OnWeaponInformationChanged;
    protected virtual void UpdateAmmoInfo()
    {
        OnWeaponInformationChanged?.Invoke(currentMagAmmo, currentAmmo);
    }

    protected void Awake()
    {
        weaponSO = (WeaponObject)itemObject;

        this.primaryAttack = weaponSO.primaryAttack;
        this.secondaryAttack = weaponSO.secondaryAttack;
        this.useAmmo = weaponSO.useAmmo;

        this.currentMagAmmo = weaponSO.magAmmo;
        this.currentMagMaxAmmo = weaponSO.magAmmo;

        this.currentAmmo = weaponSO.maxAmmo;
        this.maxAmmo = weaponSO.maxAmmo;
    }

    protected void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 1 / primaryAttack.attackRate;
    }
    public abstract void PrimaryFireInput();
    protected virtual bool canFire()
    {
        return currentMagAmmo > 0 && !isReloading;
    }
    public abstract void PrimaryFire();
    public abstract void SecondaryAttack();
    protected abstract void SpawnAttack(GameObject attackObject, Transform attackPosition, float projectileSpeed);
    public virtual void AnimPrimaryFire()
    {
        PrimaryFire();
    }
    protected abstract void ConsumeAmmo();
    public void Reload()
    {
        bool canReload = !isReloading && currentMagAmmo < currentMagMaxAmmo && currentAmmo > 0;

        if (Input.GetKeyDown(KeyCode.R) && canReload)
        {
            isReloading = true;
            anim.Play("Reload");
        }
    }

    public void FillWeaponMagazine()
    {
        int amountToReload = ReloadAmount();
        currentMagAmmo += amountToReload;
        currentAmmo -= amountToReload;

        isReloading = false;
    }

    private int ReloadAmount()
    {
        int countToFullMag = currentMagMaxAmmo - currentMagAmmo;

        return currentAmmo < countToFullMag ? currentAmmo : countToFullMag;
    }

    public void GainAmmo(int amount)
    {
        currentAmmo += amount;
        UpdateAmmoInfo();
    }
}
