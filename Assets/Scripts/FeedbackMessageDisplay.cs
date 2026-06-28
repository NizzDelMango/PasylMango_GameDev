using UdonSharp;
using UnityEngine;
using TMPro;

public class FeedbackMessageDisplay : UdonSharpBehaviour
{
    public GameObject panelObject;
    public TextMeshProUGUI messageText;

    public float hideDelay = 2.5f;

    private bool isShowing;
    private float hideAtTime;

    private void Start()
    {
        HideNow();
    }

    private void Update()
    {
        if (!isShowing) return;

        if (Time.time >= hideAtTime)
        {
            HideNow();
        }
    }

    public void ShowMessage(string message)
    {
        if (hideDelay < 0f)
        {
            hideDelay = 0f;
        }

        hideAtTime = Time.time + hideDelay;
        isShowing = true;

        if (panelObject != null)
        {
            panelObject.SetActive(true);
        }

        if (messageText != null)
        {
            messageText.text = message;
        }

        Debug.Log("[FeedbackMessageDisplay] 메시지 표시: " + message);
    }

    public void HideNow()
    {
        isShowing = false;

        if (panelObject != null)
        {
            panelObject.SetActive(false);
        }
    }
}