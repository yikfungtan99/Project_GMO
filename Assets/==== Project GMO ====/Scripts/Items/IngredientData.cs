using UnityEngine;

[System.Serializable]
public class IngredientData : ItemData
{
    public new IngredientObject itemObject;

    public int ingredientValue;
    public IngredientFamily ingredientFamily;

    public IngredientData(IngredientObject itemObject = null) : base(itemObject)
    {
        ingredientFamily = itemObject.ingredientFamily;
        ingredientValue = itemObject.ingredientValue;
    }
}
