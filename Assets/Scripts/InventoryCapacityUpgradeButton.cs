using UdonSharp;
using UnityEngine;

public class InventoryCapacityUpgradeButton : UdonSharpBehaviour
{
    public MoneyManager moneyManager;
    public SalvageInventory inventory;

    public int price = 100;
    public int capacityIncrease = 2;

    public GameObject objectToDisableAfterPurchase;

    public bool purchased;

    public override void Interact()
    {
        if (purchased)
        {
            Debug.Log("[InventoryCapacityUpgradeButton] 이미 구매한 업그레이드");
            return;
        }

        if (moneyManager == null)
        {
            Debug.Log("[InventoryCapacityUpgradeButton] moneyManager가 연결되지 않음");
            return;
        }

        if (inventory == null)
        {
            Debug.Log("[InventoryCapacityUpgradeButton] inventory가 연결되지 않음");
            return;
        }

        bool success = moneyManager.SpendMoney(price);

        if (!success)
        {
            Debug.Log("[InventoryCapacityUpgradeButton] 돈 부족 / 필요 금액: " + price);
            return;
        }

        purchased = true;

        inventory.IncreaseCapacity(capacityIncrease);

        if (objectToDisableAfterPurchase != null)
        {
            objectToDisableAfterPurchase.SetActive(false);
        }

        Debug.Log("[InventoryCapacityUpgradeButton] 가방 업그레이드 구매 성공 / +" + capacityIncrease);
    }
}