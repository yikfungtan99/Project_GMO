using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedBuff", menuName = "Project_GMO/Buff/AttackSpeedBuff")]
public class AttackSpeedBuffObject : BuffObject
{
    public int speedAmount;

    public override BuffData InitializeBuff(GameObject obj)
    {
        return new AttackSpeedBuffData(this, obj);
    }
}