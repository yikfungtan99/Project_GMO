using UnityEngine;

[System.Serializable]
public abstract class BuffData
{
    protected float Duration;
    protected int EffectStacks;
    public BuffObject Buff { get; }
    protected readonly GameObject Obj;
    public bool IsFinished;

    public BuffData(BuffObject buff, GameObject obj)
    {
        Buff = buff;
        Obj = obj;
    }

    public void Tick(float delta)
    {
        Duration -= delta;
        if (Duration <= 0)
        {
            End();
            IsFinished = true;
        }
    }

    /**
     * Activates buff or extends duration if ScriptableBuff has IsDurationStacked or IsEffectStacked set to true.
     */
    public void Activate()
    {
        if (Buff.IsEffectStacked || Duration <= 0)
        {
            ApplyEffect();
            EffectStacks++;
        }

        if (Buff.IsDurationStacked || Duration <= 0)
        {
            Duration += Buff.Duration;
        }
    }
    protected abstract void ApplyEffect();
    protected abstract void End();
}
