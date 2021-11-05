using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaxHealthBuff", menuName = "Project_GMO/Buff/MaxHealthBuff")]
public class MaxHealthBuff : BuffObject
{
    public int maxHealthModdifyAmount;

    public override BuffData InitializeBuff(GameObject obj)
    {
        return new MaxHealthBuffData(this, obj);
    }
}
