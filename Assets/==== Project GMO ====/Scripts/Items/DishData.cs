using System.Collections.Generic;

[System.Serializable]
public class DishData : ItemData
{
    public List<BuffObject> buffs { get; private set; }

    public DishData(DishObject itemObject = null) : base(itemObject)
    {
        buffs = itemObject.Buffs;
    }
}
