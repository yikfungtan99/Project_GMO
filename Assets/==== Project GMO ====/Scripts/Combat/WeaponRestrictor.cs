using UnityEngine;

[RequireComponent(typeof(WeaponComponent))]
public abstract class WeaponRestrictor : MonoBehaviour
{
    public abstract void ReplenishWeapon();
    public abstract bool RestrictFire();
    public abstract void ExhaustWeapon();
}
