using UdonSharp;
using UnityEngine;

public class InventoryCapacityUpgradeButton : UdonSharpBehaviour
{
    public MoneyManager moneyManager;
    public SalvageInventory inventory;
    public FeedbackMessageDisplay feedbackDisplay;

    public int price = 100;
    public int capacityIncrease = 2;

    public GameObject objectToDisableAfterPurchase;

    public bool purchased;

    public override void Interact()
    {
        if (purchased)
        {
            ShowFeedback("이미 구매한 가방 업그레이드입니다.");
            Debug.Log("[InventoryCapacityUpgradeButton] 이미 구매한 업그레이드");
            return;
        }

        if (price <= 0)
        {
            ShowFeedback("가방 업그레이드 가격이 올바르지 않습니다.");
            Debug.Log("[InventoryCapacityUpgradeButton] price <= 0: " + price);
            return;
        }

        if (capacityIncrease <= 0)
        {
            ShowFeedback("가방 증가량이 올바르지 않습니다.");
            Debug.Log("[InventoryCapacityUpgradeButton] capacityIncrease <= 0: " + capacityIncrease);
            return;
        }

        if (moneyManager == null)
        {
            ShowFeedback("돈 관리자가 연결되지 않았습니다.");
            Debug.Log("[InventoryCapacityUpgradeButton] moneyManager가 연결되지 않음");
            return;
        }

        if (inventory == null)
        {
            ShowFeedback("가방 관리자가 연결되지 않았습니다.");
            Debug.Log("[InventoryCapacityUpgradeButton] inventory가 연결되지 않음");
            return;
        }

        bool success = moneyManager.SpendMoney(price);

        if (!success)
        {
            ShowFeedback("돈이 부족합니다. 필요 돈: " + price);
            Debug.Log("[InventoryCapacityUpgradeButton] 돈 부족 / 필요 금액: " + price);
            return;
        }

        inventory.IncreaseCapacity(capacityIncrease);

        purchased = true;

        ShowFeedback("가방 용량이 증가했습니다. +" + capacityIncrease);

        if (objectToDisableAfterPurchase != null)
        {
            objectToDisableAfterPurchase.SetActive(false);
        }

        Debug.Log("[InventoryCapacityUpgradeButton] 가방 업그레이드 구매 성공 / +" + capacityIncrease);
    }

    private void ShowFeedback(string message)
    {
        if (feedbackDisplay != null)
        {
            feedbackDisplay.ShowMessage(message);
        }
    }
}