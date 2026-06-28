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
        if (amount <= 0)
        {
            Debug.Log("[MoneyManager] AddMoney 실패 / amount <= 0: " + amount);
            return;
        }

        money += amount;

        Debug.Log("[MoneyManager] +" + amount + " 현재 돈: " + money);

        RefreshUI();
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= 0)
        {
            Debug.Log("[MoneyManager] SpendMoney 실패 / amount <= 0: " + amount);
            return false;
        }

        if (money < amount)
        {
            Debug.Log("[MoneyManager] 돈 부족 / 필요 금액: " + amount + " 현재 돈: " + money);
            return false;
        }

        money -= amount;

        Debug.Log("[MoneyManager] -" + amount + " 현재 돈: " + money);

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