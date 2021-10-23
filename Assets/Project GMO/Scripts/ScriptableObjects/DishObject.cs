using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DishSO", menuName = "Project_GMO/Dish")]
public class DishObject : ItemObject
{
    public List<ItemObject> ingredient = new List<ItemObject>();
}
