using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class InventoryDepositBox : UdonSharpBehaviour
{
    public SalvageInventory inventory;
    public FeedbackMessageDisplay feedbackDisplay;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("[InventoryDepositBox] 무언가 들어옴: " + other.gameObject.name);

        if (inventory == null)
        {
            ShowFeedback("가방 관리자가 연결되지 않았습니다.");
            Debug.Log("[InventoryDepositBox] inventory가 연결되지 않음");
            return;
        }

        SalvageItem item = other.gameObject.GetComponent<SalvageItem>();

        if (item == null)
        {
            Debug.Log("[InventoryDepositBox] SalvageItem 아님");
            return;
        }

        if (item.isBeingSold)
        {
            ShowFeedback("이미 처리 중인 아이템입니다.");
            Debug.Log("[InventoryDepositBox] 이미 처리 중인 아이템: " + item.itemName);
            return;
        }

        bool stored = inventory.TryStoreItem(item);

        if (!stored)
        {
            string failMessage = inventory.GetLastStoreFailMessage();

            if (failMessage == null || failMessage == "")
            {
                failMessage = "아이템을 보관할 수 없습니다.";
            }

            ShowFeedback(failMessage);
            return;
        }

        VRC_Pickup pickup = other.gameObject.GetComponent<VRC_Pickup>();

        if (pickup != null)
        {
            pickup.Drop();
        }

        other.gameObject.SetActive(false);

        ShowFeedback("가방에 보관했습니다: " + item.itemName);
    }

    private void ShowFeedback(string message)
    {
        if (feedbackDisplay != null)
        {
            feedbackDisplay.ShowMessage(message);
        }
    }
}