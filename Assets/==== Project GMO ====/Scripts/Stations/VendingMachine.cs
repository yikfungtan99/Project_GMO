using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour, ICanBeInteracted
{
    [SerializeField] private ItemObject itemInside;

    private Dispenser dispenser;

    private float lookPercentage;
    public float LookPercentage { get => lookPercentage; set => lookPercentage = value; }

    // Start is called before the first frame update
    void Start()
    {
        dispenser = GetComponent<Dispenser>();
        SetInteractable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInteractable()
    {
        SelectionManager.Instance.AddInteractables(this);
    }

    public void ReceiveInteract(PlayerInteract interactor = null)
    {
        dispenser.Dispense(itemInside, 3);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void HighlightInteractable()
    {
        GetComponent<Outline>().enabled = true;
    }

    public void DeHighlightInteractable()
    {
        GetComponent<Outline>().enabled = false;
    }
}

