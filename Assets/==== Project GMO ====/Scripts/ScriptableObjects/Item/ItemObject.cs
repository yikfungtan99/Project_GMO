using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Project_GMO/Item")]
public class ItemObject : ScriptableObject
{
    [Header("Item")]
    public string itemName;
    public ItemRarity itemRarity;
    public int itemPrice;
    public bool stackable;
    public int stackMaxSize;

    public Sprite itemImage;
}
