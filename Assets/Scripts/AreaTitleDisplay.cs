using UdonSharp;
using UnityEngine;
using TMPro;

public class AreaTitleDisplay : UdonSharpBehaviour
{
    public GameObject panelObject;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;

    public float hideDelay = 3f;

    private void Start()
    {
        HideNow();
    }

    public void ShowAreaTitle(string title, string subtitle)
    {
        if (panelObject != null)
        {
            panelObject.SetActive(true);
        }

        if (titleText != null)
        {
            titleText.text = title;
        }

        if (subtitleText != null)
        {
            subtitleText.text = subtitle;
        }

        Debug.Log("[AreaTitleDisplay] 지역 표시: " + title);

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