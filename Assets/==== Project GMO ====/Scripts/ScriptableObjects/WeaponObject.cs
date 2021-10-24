using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Project_GMO/Weapon")]
public class WeaponObject : ItemObject
{
    public Attack primaryAttack;
    public Attack secondaryAttack;
    public bool useAmmo = false;
    public int currentAmmo;
    public int maxAmmo;
}
