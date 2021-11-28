using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private GameDirector gameDirector;
    [SerializeField] private TextMeshProUGUI moneyText;

    public delegate void MoneyChangeCallback();

    public event MoneyChangeCallback OnAddMoney;
    public event MoneyChangeCallback OnLoseMoney;

    public int money { get; private set; }

    private void Start()
    {
        EarnMoney(9999);
    }

    public void SellItem(ItemData item)
    {
        EarnMoney(item.itemPrice); 
    }

    public void EarnMoney(int amount)
    {
        money += amount;
        moneyText.text = money.ToString();

        OnAddMoney?.Invoke();
    }

    public void SpendMoney(int amount)
    {
        money -= amount;
        moneyText.text = money.ToString();

        OnLoseMoney?.Invoke();
    }

    public bool CanAfford(int amount)
    {
        return money >= amount;
    }
}
