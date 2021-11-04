[System.Serializable]
public class DishData : ItemData
{
    public BuffObject buff { get; private set; }

    public DishData(DishObject itemObject = null) : base(itemObject)
    {
        buff = itemObject.Buff;
    }
}
