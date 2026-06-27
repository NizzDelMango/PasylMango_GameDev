using UdonSharp;
using UnityEngine;

public class SellBox : UdonSharpBehaviour
{
    public MoneyManager moneyManager;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("[SellBox] 무언가 들어옴: " + other.gameObject.name);

        SalvageItem item = other.gameObject.GetComponent<SalvageItem>();

        if (item == null)
        {
            Debug.Log("[SellBox] SalvageItem 아님");
            return;
        }

        if (item.isPlaced)
        {
            Debug.Log("[SellBox] 이미 장식된 아이템이라 판매 안 함");
            return;
        }

        if (moneyManager == null)
        {
            Debug.Log("[SellBox] moneyManager가 연결되지 않음");
            return;
        }

        Debug.Log("[SellBox] 판매 성공: " + item.itemName + " / 가격: " + item.sellPrice);

        moneyManager.AddMoney(item.sellPrice);

        other.gameObject.SetActive(false);
    }
}