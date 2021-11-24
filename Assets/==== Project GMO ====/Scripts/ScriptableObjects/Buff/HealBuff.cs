using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealBuff", menuName = "Project_GMO/Buff/HealBuff")]
public class HealBuff : BuffObject
{
    public int healAmount;

    public override BuffData InitializeBuff(GameObject obj)
    {
        return new HealBuffData(this, obj);
    }
}