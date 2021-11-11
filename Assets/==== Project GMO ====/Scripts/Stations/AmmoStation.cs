using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoStation : Station, ICanBeInteracted
{
    [SerializeField] private int price;
    [SerializeField] private int ammo;

    private MoneyManager moneyManager;

    private float lookPercentage;
    public float LookPercentage { get => lookPercentage; set => lookPercentage = value; }

    // Start is called before the first frame update
    void Start()
    {
        moneyManager = GameObject.FindGameObjectWithTag("MoneyManager").GetComponent<MoneyManager>();
        SetInteractable();
    }

    public void ReceiveInteract(PlayerInteract interactor)
    {
        if (moneyManager.CanAfford(price))
        {
            interactor.GetComponent<PlayerCombat>().GainAmmo(ammo);
            moneyManager.SpendMoney(price);
        }
    }

    public void SetInteractable()
    {
        SelectionManager.Instance.AddInteractables(this);
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
