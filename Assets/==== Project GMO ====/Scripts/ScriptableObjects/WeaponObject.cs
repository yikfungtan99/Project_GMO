using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Project_GMO/Weapon")]
public class WeaponObject : ItemObject
{
    [Header("Weapon")]
    public GameObject weaponGameObject;

    [Header("Weapon Ammo")]
    public bool useAmmo = false;
    public int maxMagAmmo;
    public int maxAmmo;

    [Header("Weapon Attack")]
    public Attack primaryAttack;
    public Attack secondaryAttack;

}
