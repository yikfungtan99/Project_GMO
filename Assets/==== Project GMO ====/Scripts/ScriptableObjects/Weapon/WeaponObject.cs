using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Project_GMO/Weapon")]
public class WeaponObject : ItemObject
{
    [Header("Weapon")]
    public GameObject weaponGameObject;

    [Header("Weapon Attack")]
    public Attack primaryAttack;
    public Attack secondaryAttack;

}
