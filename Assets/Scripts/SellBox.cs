using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class SellBox : UdonSharpBehaviour
{
    public MoneyManager moneyManager;
    public FeedbackMessageDisplay feedbackDisplay;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("[SellBox] 무언가 들어옴: " + other.gameObject.name);

        SalvageItem item = other.gameObject.GetComponent<SalvageItem>();

        if (item == null)
        {
            Debug.Log("[SellBox] SalvageItem 아님");
            return;
        }

        if (item.IsBeingSold())
        {
            ShowFeedback("이미 판매 처리 중인 아이템입니다.");
            Debug.Log("[SellBox] 이미 판매 처리 중인 아이템: " + item.itemName);
            return;
        }

        if (item.IsPlaced())
        {
            ShowFeedback("장식된 아이템은 판매할 수 없습니다.");
            Debug.Log("[SellBox] 이미 장식된 아이템이라 판매 안 함");
            return;
        }

        if (item.IsInInventory())
        {
            ShowFeedback("가방에 들어간 아이템은 직접 판매할 수 없습니다.");
            Debug.Log("[SellBox] 가방에 들어간 아이템이라 직접 판매 안 함: " + item.itemName);
            return;
        }

        if (item.sellPrice <= 0)
        {
            ShowFeedback("판매가가 올바르지 않은 아이템입니다.");
            Debug.Log("[SellBox] sellPrice <= 0: " + item.itemName + " / 가격: " + item.sellPrice);
            return;
        }

        if (moneyManager == null)
        {
            ShowFeedback("정화칩 관리자가 연결되지 않았습니다.");
            Debug.Log("[SellBox] moneyManager가 연결되지 않음");
            return;
        }

        item.MarkSelling();

        Debug.Log("[SellBox] 판매 성공: " + item.itemName + " / 가격: " + item.sellPrice);

        moneyManager.AddMoney(item.sellPrice);

        VRC_Pickup pickup = other.gameObject.GetComponent<VRC_Pickup>();

        if (pickup != null)
        {
            pickup.Drop();
        }

        if (item.respawner != null)
        {
            item.respawner.RespawnLater();
        }
        else
        {
            Debug.Log("[SellBox] respawner가 없는 아이템. 리스폰 없음: " + item.itemName);
        }

        other.gameObject.SetActive(false);

        ShowFeedback("판매 완료: " + item.itemName + " +" + item.sellPrice);
    }

    private void ShowFeedback(string message)
    {
        if (feedbackDisplay != null)
        {
            feedbackDisplay.ShowMessage(message);
        }
    }
}