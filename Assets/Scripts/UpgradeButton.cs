using UdonSharp;
using UnityEngine;

public class UpgradeButton : UdonSharpBehaviour
{
    public MoneyManager moneyManager;

    public int price = 100;
    public GameObject objectToUnlock;
    public GameObject objectToDisableAfterPurchase;

    public bool purchased;

    public override void Interact()
    {
        if (purchased)
        {
            Debug.Log("[UpgradeButton] 이미 구매한 업그레이드");
            return;
        }

        if (moneyManager == null)
        {
            Debug.Log("[UpgradeButton] moneyManager가 연결되지 않음");
            return;
        }

        if (objectToUnlock == null)
        {
            Debug.Log("[UpgradeButton] objectToUnlock이 연결되지 않음");
            return;
        }

        bool success = moneyManager.SpendMoney(price);

        if (!success)
        {
            Debug.Log("[UpgradeButton] 돈 부족 / 필요 금액: " + price);
            return;
        }

        objectToUnlock.SetActive(true);

        if (objectToDisableAfterPurchase != null)
        {
            objectToDisableAfterPurchase.SetActive(false);
        }

        purchased = true;

        Debug.Log("[UpgradeButton] 업그레이드 구매 성공 / 가격: " + price);
    }
}