using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientSO", menuName = "Project_GMO/Ingredient")]
public class IngredientObject : ItemObject
{
    public IngredientFamily ingredientFamily;
    public int ingredientValue;
}

