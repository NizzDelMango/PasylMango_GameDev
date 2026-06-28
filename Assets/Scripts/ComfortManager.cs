using UdonSharp;
using UnityEngine;
using TMPro;

public class ComfortManager : UdonSharpBehaviour
{
    public int comfortScore;
    public TextMeshProUGUI comfortText;

    private void Start()
    {
        RefreshUI();
    }

    public void AddComfort(int amount)
    {
        if (amount <= 0)
        {
            Debug.Log("[ComfortManager] 0 이하 점수는 추가하지 않음");
            return;
        }

        comfortScore += amount;

        Debug.Log("[ComfortManager] 아늑함 증가: +" + amount + " / 현재: " + comfortScore);

        RefreshUI();
    }

    public bool HasComfort(int requiredAmount)
    {
        return comfortScore >= requiredAmount;
    }

    private void RefreshUI()
    {
        if (comfortText != null)
        {
            comfortText.text = "아늑함: " + comfortScore;
        }
    }
}