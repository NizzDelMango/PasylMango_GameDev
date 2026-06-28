using UdonSharp;
using UnityEngine;
using TMPro;

public class ExplorationAreaInfoSign : UdonSharpBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;

    public string areaTitle = "탐사 지역";
    public string difficultyText = "초급 탐사 지역";
    public string statusText = "이동 가능";
    public string rewardText = "주요 발견물 없음";
    public string unlockConditionText = "";

    private void Start()
    {
        RefreshSign();
    }

    public void RefreshSign()
    {
        if (titleText != null)
        {
            titleText.text = areaTitle;
        }

        if (bodyText != null)
        {
            string text = difficultyText;
            text += "\n상태: " + statusText;
            text += "\n주요 발견물: " + rewardText;

            if (unlockConditionText != null && unlockConditionText != "")
            {
                text += "\n해금 조건: " + unlockConditionText;
            }

            bodyText.text = text;
        }

        Debug.Log("[ExplorationAreaInfoSign] 안내판 갱신: " + areaTitle);
    }
}