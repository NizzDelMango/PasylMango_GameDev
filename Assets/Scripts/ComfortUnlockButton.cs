using UdonSharp;
using UnityEngine;

public class ComfortUnlockButton : UdonSharpBehaviour
{
    public ComfortManager comfortManager;

    public int requiredComfort = 5;

    public GameObject objectToUnlock;
    public GameObject objectToDisableAfterUnlock;

    public bool unlocked;

    public override void Interact()
    {
        if (unlocked)
        {
            Debug.Log("[ComfortUnlockButton] 이미 해금됨");
            return;
        }

        if (comfortManager == null)
        {
            Debug.Log("[ComfortUnlockButton] comfortManager가 연결되지 않음");
            return;
        }

        if (objectToUnlock == null)
        {
            Debug.Log("[ComfortUnlockButton] objectToUnlock이 연결되지 않음");
            return;
        }

        if (!comfortManager.HasComfort(requiredComfort))
        {
            Debug.Log("[ComfortUnlockButton] 아늑함 부족 / 필요 아늑함: " + requiredComfort);
            return;
        }

        unlocked = true;

        objectToUnlock.SetActive(true);

        if (objectToDisableAfterUnlock != null)
        {
            objectToDisableAfterUnlock.SetActive(false);
        }

        Debug.Log("[ComfortUnlockButton] 아늑함 해금 성공 / 필요 아늑함: " + requiredComfort);
    }
}