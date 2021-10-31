using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour, ICanBeInteracted
{
    [SerializeField] private ItemObject itemInside;

    private Dispenser dispenser;

    // Start is called before the first frame update
    void Start()
    {
        dispenser = GetComponent<Dispenser>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveInteract(PlayerInteract interactor = null)
    {
        dispenser.Dispense(itemInside, 3);
    }
}

