using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dispenser))]
public class CraftingStation : Station, ICanPickUpItems
{
    [SerializeField] private List<RecipeObject> availableRecipes = new List<RecipeObject>();
    [SerializeField] private List<IngredientObject> items = new List<IngredientObject>();
    [SerializeField] private int maxItemSlot = 5;

    [Tooltip("Time to wait for other Ingredient to come in for the first ingredient")]
    [SerializeField] private float baseCraftingDuration = 5;
    private float currentCraftingDuration;
    private float currentCraftTime;
    [SerializeField] private List<ItemObject> resultItems = new List<ItemObject>();

    private RecipeObject currentCraftingRecipe;

    private Dispenser dispenser;

    public delegate void CraftingStationDele();
    public event CraftingStationDele OnAddItem;
    public event CraftingStationDele OnFinishCrafting;

    [SerializeField] private Slider progressBar;

    [SerializeField] private GameObject craftingImagePrefab;

    [SerializeField] private GameObject craftingUICanvas;
    [SerializeField] private Transform ingredientRack;

    private void OnEnable()
    {
        OnFinishCrafting += DispenseResult;
        OnFinishCrafting += ResetCraftingStation;
        OnFinishCrafting += RemoveIngredientUI;
        OnFinishCrafting += UpdateProgressBar;

        OnAddItem += GetCompatibleRecipe;
        OnAddItem += AddIngredientUI;
    }

    private void Start()
    {
        dispenser = GetComponent<Dispenser>();
        currentCraftingDuration = baseCraftingDuration;
        currentCraftTime = 0;
    }

    private void Update()
    {
        Crafting();
    }

    private void Crafting()
    {
        if (items.Count > 0)
        {
            if(currentCraftTime <= currentCraftingDuration)
            {
                currentCraftTime += Time.deltaTime;
            }
            else
            {
                if(currentCraftingRecipe != null)
                {
                    resultItems.Add(currentCraftingRecipe.item);
                }
                else
                {
                    resultItems = items.ConvertAll(x => (ItemObject)x);
                }

                OnFinishCrafting?.Invoke();
            }

            UpdateProgressBar();
        }
    }

    private void GetCompatibleRecipe()
    {
        List<RecipeObject> possibleRecipes = new List<RecipeObject>();

        foreach (RecipeObject checkingRecipe in availableRecipes)
        {
            if (CompareLists(items, checkingRecipe.requiredIngredients))
            {
                possibleRecipes.Add(checkingRecipe);
            }
        }

        if(possibleRecipes.Count > 0)
        {
            currentCraftingRecipe = possibleRecipes.First();
            currentCraftingDuration += currentCraftingRecipe.craftDuration;
        }
        else
        {
            currentCraftingRecipe = null;
        }
    }

    private void DispenseResult()
    {
        foreach (ItemObject item in resultItems)
        {
            dispenser.Dispense(item);
        }

        ResetCraftingStation();
    }

    private void ResetCraftingStation()
    {
        currentCraftingRecipe = null;
        resultItems.Clear();
        items.Clear();
        currentCraftTime = 0;
        currentCraftingDuration = baseCraftingDuration;
    }

    public void PickUpItem(Pickups pickup)
    {
        if (pickup.GetPickupItem().GetType() == typeof(IngredientData))
        {
            if (items.Count < maxItemSlot)
            {
                AddIngredient(pickup);
                Destroy(pickup.gameObject);
            }
        }
    }

    private void AddIngredient(Pickups pickup)
    {
        items.Add(pickup.GetPickupItem().itemObject as IngredientObject);
        OnAddItem?.Invoke();
    }

    public void UpdateProgressBar()
    {
        progressBar.value = currentCraftTime / currentCraftingDuration;
    }

    public void AddIngredientUI()
    {
        if(!craftingUICanvas.activeSelf) craftingUICanvas.SetActive(true);
        Instantiate(craftingImagePrefab, ingredientRack);
    }

    public void RemoveIngredientUI()
    {
        craftingUICanvas.SetActive(false);

        for (int i = ingredientRack.childCount - 1; i >= 0; i--)
        {
            Destroy(ingredientRack.GetChild(i).gameObject);
        }
    }

    private void OnDisable()
    {
        OnFinishCrafting -= DispenseResult;
        OnFinishCrafting -= ResetCraftingStation;
        OnFinishCrafting -= RemoveIngredientUI;
        OnFinishCrafting -= UpdateProgressBar;

        OnAddItem -= GetCompatibleRecipe;
        OnAddItem -= AddIngredientUI;
    }

    public static bool CompareLists<T>(List<T> aListA, List<T> aListB)
    {
        if (aListA == null || aListB == null || aListA.Count != aListB.Count)
            return false;
        if (aListA.Count == 0)
            return true;
        Dictionary<T, int> lookUp = new Dictionary<T, int>();
        // create index for the first list
        for (int i = 0; i < aListA.Count; i++)
        {
            int count = 0;
            if (!lookUp.TryGetValue(aListA[i], out count))
            {
                lookUp.Add(aListA[i], 1);
                continue;
            }
            lookUp[aListA[i]] = count + 1;
        }
        for (int i = 0; i < aListB.Count; i++)
        {
            int count = 0;
            if (!lookUp.TryGetValue(aListB[i], out count))
            {
                // early exit as the current value in B doesn't exist in the lookUp (and not in ListA)
                return false;
            }
            count--;
            if (count <= 0)
                lookUp.Remove(aListB[i]);
            else
                lookUp[aListB[i]] = count;
        }
        // if there are remaining elements in the lookUp, that means ListA contains elements that do not exist in ListB
        return lookUp.Count == 0;
    }
}
