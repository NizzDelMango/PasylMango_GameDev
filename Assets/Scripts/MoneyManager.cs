using UdonSharp;
using UnityEngine;
using TMPro;

public class MoneyManager : UdonSharpBehaviour
{
    public int money;
    public TextMeshProUGUI moneyText;

    private void Start()
    {
        RefreshUI();
    }

    public void AddMoney(int amount)
    {
        money += amount;

        Debug.Log("[MoneyManager] +" + amount + " 현재 돈: " + money);

        RefreshUI();
    }

    public bool SpendMoney(int amount)
    {
        if (money < amount)
        {
            Debug.Log("[MoneyManager] 돈 부족");
            return false;
        }

        money -= amount;
        RefreshUI();
        return true;
    }

    private void RefreshUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "현재 돈: " + money;
        }
        else
        {
            Debug.Log("[MoneyManager] moneyText가 연결되지 않음");
        }
    }
}