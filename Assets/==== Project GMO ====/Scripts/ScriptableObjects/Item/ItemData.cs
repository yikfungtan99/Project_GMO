using UnityEngine;

[System.Serializable]
public class ItemData
{
    public ItemObject itemObject;

    public string itemName;
    public ItemRarity itemRarity;
    public int itemPrice;

    public bool stackable;
    public int currentStack = 1;
    public int stackMaxSize;

    public ItemData(ItemObject itemObject = null)
    {
        if (itemObject != null)
        {
            this.itemObject = itemObject;
            this.itemName = itemObject.itemName;
            this.itemRarity = itemObject.itemRarity;
            this.itemPrice = itemObject.itemPrice;
        }
    }
}
