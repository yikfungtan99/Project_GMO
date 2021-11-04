using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoStation : Station, ICanBeInteracted
{
    [SerializeField] private int price;
    [SerializeField] private int ammo;

    private MoneyManager moneyManager;

    // Start is called before the first frame update
    void Start()
    {
        moneyManager = GameObject.FindGameObjectWithTag("MoneyManager").GetComponent<MoneyManager>();       
    }

    public void ReceiveInteract(PlayerInteract interactor)
    {
        if (moneyManager.CanAfford(price))
        {
            interactor.GetComponent<PlayerCombat>().GainAmmo(ammo);
            moneyManager.SpendMoney(price);
        }
    }
}
