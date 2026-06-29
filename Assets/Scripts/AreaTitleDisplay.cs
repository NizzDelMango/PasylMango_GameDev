using UdonSharp;
using UnityEngine;
using TMPro;

public class AreaTitleDisplay : UdonSharpBehaviour
{
    public GameObject panelObject;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;

    public float hideDelay = 3f;

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

    public void ShowAreaTitle(string title, string subtitle)
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

        if (titleText != null)
        {
            titleText.text = title;
        }

        if (subtitleText != null)
        {
            subtitleText.text = subtitle;
        }

        Debug.Log("[AreaTitleDisplay] 지역 표시: " + title);
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