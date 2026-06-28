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

    public bool unlocked;

    public override void Interact()
    {
        if (unlocked)
        {
            Debug.Log("[ExplorationAreaUnlockButton] 이미 해금됨");
            return;
        }

        if (objectToUnlock == null)
        {
            Debug.Log("[ExplorationAreaUnlockButton] objectToUnlock이 연결되지 않음");
            return;
        }

        if (requireComfort)
        {
            if (comfortManager == null)
            {
                Debug.Log("[ExplorationAreaUnlockButton] comfortManager가 연결되지 않음");
                return;
            }

            if (!comfortManager.HasComfort(requiredComfort))
            {
                Debug.Log("[ExplorationAreaUnlockButton] 아늑함 부족 / 필요 아늑함: " + requiredComfort);
                return;
            }
        }

        if (requireMoney)
        {
            if (moneyManager == null)
            {
                Debug.Log("[ExplorationAreaUnlockButton] moneyManager가 연결되지 않음");
                return;
            }

            bool success = moneyManager.SpendMoney(price);

            if (!success)
            {
                Debug.Log("[ExplorationAreaUnlockButton] 돈 부족 / 필요 금액: " + price);
                return;
            }
        }

        unlocked = true;

        objectToUnlock.SetActive(true);

        if (objectToDisableAfterUnlock != null)
        {
            objectToDisableAfterUnlock.SetActive(false);
        }

        Debug.Log("[ExplorationAreaUnlockButton] 탐사 지역 해금 성공: " + objectToUnlock.name);
    }
}