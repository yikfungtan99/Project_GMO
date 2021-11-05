using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player), typeof(BuffProcessor))]
public class PlayerEat : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;

    private Player player;
    private BuffProcessor buffProcessor;

    private void Start()
    {
        player = GetComponent<Player>();
        buffProcessor = GetComponent<BuffProcessor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DishData dish = inventory.GetSelectedItem() as DishData;

            if (dish != null)
            {
                foreach (BuffObject buff in dish.buffs)
                {
                    buffProcessor.AddBuff(buff.InitializeBuff(gameObject));
                }

                inventory.RemoveSelectedItem();
            }
        }
    }
}
