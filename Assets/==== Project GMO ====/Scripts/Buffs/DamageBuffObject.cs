using UnityEngine;

[CreateAssetMenu(fileName = "DamageBuff", menuName = "Project_GMO/Buff/DamageBuff")]
public class DamageBuffObject : BuffObject
{
    public int damageAmount;

    public override BuffData InitializeBuff(GameObject obj)
    {
        return new DamageBuffData(this, obj);
    }
}
