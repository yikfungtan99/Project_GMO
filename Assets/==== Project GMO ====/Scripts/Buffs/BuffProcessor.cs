using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffProcessor : MonoBehaviour
{
    private readonly Dictionary<ScriptableObject, BuffData> _buffs = new Dictionary<ScriptableObject, BuffData>();

    void Update()
    {
        foreach (var buff in _buffs.Values.ToList())
        {
            buff.Tick(Time.deltaTime);
            if (buff.IsFinished)
            {
                _buffs.Remove(buff.Buff);
            }
        }
    }

    public void AddBuff(BuffData buff)
    {
        if (_buffs.ContainsKey(buff.Buff))
        {
            _buffs[buff.Buff].Activate();
        }
        else
        {
            _buffs.Add(buff.Buff, buff);
            buff.Activate();
        }
    }
}
