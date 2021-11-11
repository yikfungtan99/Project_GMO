using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SandwichUtilities;

[System.Serializable]
public struct Drops
{
    public ItemObject itemDrops;
    public int chance;
}

[RequireComponent(typeof(Enemy), typeof(Dispenser))]
public class EnemyDrop : MonoBehaviour
{
    [SerializeField] private List<Drops> randomItemDrops;

    private Enemy enemy;
    private Dispenser dispenser;

    public int times;

    List<int> weightage = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        dispenser = GetComponent<Dispenser>();

        enemy.OnDeath += DropItem;

        if(randomItemDrops.Count > 0)
        {
            foreach (Drops item in randomItemDrops)
            {
                weightage.Add(item.chance);
            }
        }
    }

    private void DropItem(Enemy enemy)
    {
        ItemObject dropItem = randomItemDrops[UtilScripts.RandomByWeightage(weightage)].itemDrops;

        if (dropItem != null)
        {
            dispenser.Dispense(dropItem);
        }
    }
}

