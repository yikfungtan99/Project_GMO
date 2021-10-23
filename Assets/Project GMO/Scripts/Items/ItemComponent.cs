using System.Collections;
using UnityEngine;

public enum ItemRarity 
{ 
    
}

public abstract class ItemComponent : MonoBehaviour
{
    [SerializeField] protected ItemObject itemObject;
    protected ItemData itemData;
}
