using UdonSharp;
using UnityEngine;
using TMPro;

public class FeedbackMessageDisplay : UdonSharpBehaviour
{
    public GameObject panelObject;
    public TextMeshProUGUI messageText;

    public float hideDelay = 2.5f;

    private void Start()
    {
        HideNow();
    }

    public void ShowMessage(string message)
    {
        if (panelObject != null)
        {
            panelObject.SetActive(true);
        }

        if (messageText != null)
        {
            messageText.text = message;
        }

        Debug.Log("[FeedbackMessageDisplay] 메시지 표시: " + message);

        SendCustomEventDelayedSeconds("HideNow", hideDelay);
    }

    public void HideNow()
    {
        if (panelObject != null)
        {
            panelObject.SetActive(false);
        }
    }
}