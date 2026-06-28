using UdonSharp;
using UnityEngine;

public class InventorySellButton : UdonSharpBehaviour
{
    public SalvageInventory inventory;
    public FeedbackMessageDisplay feedbackDisplay;

    public override void Interact()
    {
        if (inventory == null)
        {
            ShowFeedback("가방 관리자가 연결되지 않았습니다.");
            Debug.Log("[InventorySellButton] inventory가 연결되지 않음");
            return;
        }

        if (!inventory.HasStoredItems())
        {
            ShowFeedback("가방에 판매할 아이템이 없습니다.");
            Debug.Log("[InventorySellButton] 판매할 아이템 없음");
            return;
        }

        inventory.SellAllStoredItems();

        ShowFeedback("가방 안 아이템을 판매했습니다.");
    }

    private void ShowFeedback(string message)
    {
        if (feedbackDisplay != null)
        {
            feedbackDisplay.ShowMessage(message);
        }
    }
}