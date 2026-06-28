using UdonSharp;
using UnityEngine;

public class ComfortUnlockButton : UdonSharpBehaviour
{
    public ComfortManager comfortManager;
    public FeedbackMessageDisplay feedbackDisplay;

    public int requiredComfort = 5;

    public GameObject objectToUnlock;
    public GameObject objectToDisableAfterUnlock;

    public bool unlocked;

    public override void Interact()
    {
        if (unlocked)
        {
            ShowFeedback("이미 해금된 항목입니다.");
            Debug.Log("[ComfortUnlockButton] 이미 해금됨");
            return;
        }

        if (requiredComfort < 0)
        {
            ShowFeedback("필요 아늑함 값이 올바르지 않습니다.");
            Debug.Log("[ComfortUnlockButton] requiredComfort < 0: " + requiredComfort);
            return;
        }

        if (comfortManager == null)
        {
            ShowFeedback("아늑함 관리자가 연결되지 않았습니다.");
            Debug.Log("[ComfortUnlockButton] comfortManager가 연결되지 않음");
            return;
        }

        if (objectToUnlock == null)
        {
            ShowFeedback("해금할 오브젝트가 연결되지 않았습니다.");
            Debug.Log("[ComfortUnlockButton] objectToUnlock이 연결되지 않음");
            return;
        }

        if (!comfortManager.HasComfort(requiredComfort))
        {
            ShowFeedback("아늑함이 부족합니다. 필요 아늑함: " + requiredComfort);
            Debug.Log("[ComfortUnlockButton] 아늑함 부족 / 필요 아늑함: " + requiredComfort);
            return;
        }

        unlocked = true;

        objectToUnlock.SetActive(true);

        ShowFeedback("아늑함 보상이 해금되었습니다.");

        if (objectToDisableAfterUnlock != null)
        {
            objectToDisableAfterUnlock.SetActive(false);
        }

        Debug.Log("[ComfortUnlockButton] 아늑함 해금 성공 / 필요 아늑함: " + requiredComfort);
    }

    private void ShowFeedback(string message)
    {
        if (feedbackDisplay != null)
        {
            feedbackDisplay.ShowMessage(message);
        }
    }
}