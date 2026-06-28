using UdonSharp;
using UnityEngine;

public class ExplorationAreaUnlockButton : UdonSharpBehaviour
{
    public MoneyManager moneyManager;
    public ComfortManager comfortManager;

    public bool requireMoney = true;
    public int price = 200;

    public bool requireComfort;
    public int requiredComfort = 10;

    public GameObject objectToUnlock;
    public GameObject objectToDisableAfterUnlock;

    public ExplorationAreaInfoSign signToUpdateAfterUnlock;
    public FeedbackMessageDisplay feedbackDisplay;

    public bool unlocked;

    public override void Interact()
    {
        if (unlocked)
        {
            ShowFeedback("이미 해금된 지역입니다.");
            Debug.Log("[ExplorationAreaUnlockButton] 이미 해금됨");
            return;
        }

        if (objectToUnlock == null)
        {
            ShowFeedback("해금 대상이 연결되지 않았습니다.");
            Debug.Log("[ExplorationAreaUnlockButton] objectToUnlock이 연결되지 않음");
            return;
        }

        if (requireComfort)
        {
            if (comfortManager == null)
            {
                ShowFeedback("아늑함 관리자가 연결되지 않았습니다.");
                Debug.Log("[ExplorationAreaUnlockButton] comfortManager가 연결되지 않음");
                return;
            }

            if (!comfortManager.HasComfort(requiredComfort))
            {
                ShowFeedback("아늑함이 부족합니다. 필요 아늑함: " + requiredComfort);
                Debug.Log("[ExplorationAreaUnlockButton] 아늑함 부족 / 필요 아늑함: " + requiredComfort);
                return;
            }
        }

        if (requireMoney)
        {
            if (moneyManager == null)
            {
                ShowFeedback("돈 관리자가 연결되지 않았습니다.");
                Debug.Log("[ExplorationAreaUnlockButton] moneyManager가 연결되지 않음");
                return;
            }

            bool success = moneyManager.SpendMoney(price);

            if (!success)
            {
                ShowFeedback("돈이 부족합니다. 필요 돈: " + price);
                Debug.Log("[ExplorationAreaUnlockButton] 돈 부족 / 필요 금액: " + price);
                return;
            }
        }

        unlocked = true;

        objectToUnlock.SetActive(true);

        if (signToUpdateAfterUnlock != null)
        {
            signToUpdateAfterUnlock.SetUnlocked();
        }

        ShowFeedback("새 탐사 지역이 해금되었습니다.");

        if (objectToDisableAfterUnlock != null)
        {
            objectToDisableAfterUnlock.SetActive(false);
        }

        Debug.Log("[ExplorationAreaUnlockButton] 탐사 지역 해금 성공: " + objectToUnlock.name);
    }

    private void ShowFeedback(string message)
    {
        if (feedbackDisplay != null)
        {
            feedbackDisplay.ShowMessage(message);
        }
    }
}