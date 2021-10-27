using UnityEngine;

[System.Serializable]
public class EggsenceData : ItemData
{
    public GameObject foodling;

    public EggsenceData(ItemObject itemObject = null) : base(itemObject)
    {
        EggsenceObject egg = itemObject as EggsenceObject;
        this.foodling = egg.foodling;
    }
}
