using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Project_GMO/Recipe")]
public class RecipeObject : ScriptableObject
{
    public ItemObject item;
    public int itemCount;
    public List<IngredientObject> requiredIngredients;
    public List<IngredientFamily> alternateRecipeRequirement;
    public List<IngredientObject> forbiddenIngredient;
    public float craftDuration;
}
