using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : WeaponRestrictor
{
    private Animator anim;

    private int currentMagAmmo;
    [SerializeField] private int currentMagMaxAmmo;

    private int currentAmmo;
    [SerializeField] private int maxAmmo;

    [SerializeField] private float reloadTime;

    public event Action<int> OnMagChanged = delegate { };
    public event Action<int> OnAmmoChanged = delegate { };

    private void Start()
    {
        anim = GetComponent<Animator>();

        currentMagAmmo = currentMagMaxAmmo;
        currentAmmo = maxAmmo;

        OnMagChanged?.Invoke(currentMagAmmo);
        OnAmmoChanged?.Invoke(currentAmmo);
    }

    public void Reload()
    {
        bool canReload = currentMagAmmo < currentMagMaxAmmo && currentAmmo > 0;

        if (Input.GetKeyDown(KeyCode.R) && canReload)
        {
            anim.speed = 1 / reloadTime;
            anim.Play("Reload");
        }
    }

    public void ConsumeAmmo()
    {
        if (currentMagAmmo > 0)
        {
            currentMagAmmo--;
        }
        else
        {
            currentMagAmmo = 0;
        }

        OnMagChanged?.Invoke(currentMagAmmo);
    }

    public override void ExhaustWeapon()
    {
        ConsumeAmmo();
    }
    public void FillWeaponMagazine()
    {
        int amountToReload = ReloadAmount();
        currentMagAmmo += amountToReload;
        currentAmmo -= amountToReload;

        OnMagChanged?.Invoke(currentMagAmmo);
        OnAmmoChanged?.Invoke(currentAmmo);
    }

    private int ReloadAmount()
    {
        int countToFullMag = currentMagMaxAmmo - currentMagAmmo;

        return currentAmmo < countToFullMag ? currentAmmo : countToFullMag;
    }

    public void GainAmmo(int amount)
    {
        currentAmmo += amount;
        OnAmmoChanged?.Invoke(currentAmmo);
    }

    public override void ReplenishWeapon()
    {
        Reload();
        OnMagChanged?.Invoke(currentMagAmmo);
        OnAmmoChanged?.Invoke(currentAmmo);
    }
    public override bool RestrictFire() => currentMagAmmo == 0;
}