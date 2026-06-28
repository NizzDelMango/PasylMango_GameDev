using UdonSharp;
using UnityEngine;

public class UpgradeButton : UdonSharpBehaviour
{
    public MoneyManager moneyManager;
    public FeedbackMessageDisplay feedbackDisplay;

    public int price = 100;
    public GameObject objectToUnlock;
    public GameObject objectToDisableAfterPurchase;

    public bool purchased;

    public override void Interact()
    {
        if (purchased)
        {
            ShowFeedback("이미 구매한 업그레이드입니다.");
            Debug.Log("[UpgradeButton] 이미 구매한 업그레이드");
            return;
        }

        if (moneyManager == null)
        {
            ShowFeedback("정화칩 관리자가 연결되지 않았습니다.");
            Debug.Log("[UpgradeButton] moneyManager가 연결되지 않음");
            return;
        }

        if (objectToUnlock == null)
        {
            ShowFeedback("해금할 오브젝트가 연결되지 않았습니다.");
            Debug.Log("[UpgradeButton] objectToUnlock이 연결되지 않음");
            return;
        }

        bool success = moneyManager.SpendMoney(price);

        if (!success)
        {
            ShowFeedback("정화칩이 부족합니다. 필요 정화칩: " + price);
            Debug.Log("[UpgradeButton] 돈 부족 / 필요 금액: " + price);
            return;
        }

        purchased = true;

        objectToUnlock.SetActive(true);

        ShowFeedback("새 선반이 해금되었습니다.");

        if (objectToDisableAfterPurchase != null)
        {
            objectToDisableAfterPurchase.SetActive(false);
        }

        Debug.Log("[UpgradeButton] 업그레이드 구매 성공 / 가격: " + price);
    }

    private void ShowFeedback(string message)
    {
        if (feedbackDisplay != null)
        {
            feedbackDisplay.ShowMessage(message);
        }
    }
}