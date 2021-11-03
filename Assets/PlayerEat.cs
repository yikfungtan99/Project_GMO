using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEat : MonoBehaviour
{

    [SerializeField] private PlayerInventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DishData dish = inventory.GetSelectedItem() as DishData;

            if (dish != null)
            {
                inventory.RemoveSelectedItem();
            }
        }
    }
}
