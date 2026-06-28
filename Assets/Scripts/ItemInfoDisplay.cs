using UdonSharp;
using UnityEngine;
using TMPro;

public class ItemInfoDisplay : UdonSharpBehaviour
{
    public GameObject panelObject;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI decorateText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI descriptionText;

    private SalvageItem currentItem;

    private void Start()
    {
        Hide();
    }

    public void ShowItem(SalvageItem item)
    {
        if (item == null)
        {
            Hide();
            return;
        }

        currentItem = item;

        if (panelObject != null)
        {
            panelObject.SetActive(true);
        }

        if (nameText != null)
        {
            nameText.text = item.itemName;
        }

        if (priceText != null)
        {
            priceText.text = "판매가: " + item.sellPrice;
        }

        if (decorateText != null)
        {
            if (item.canDecorate)
            {
                decorateText.text = "장식 가능";
            }
            else
            {
                decorateText.text = "판매 전용";
            }
        }

        if (rarityText != null)
        {
            rarityText.text = "등급: " + GetRarityName(item.rarity);
        }

        if (descriptionText != null)
        {
            if (item.description == null || item.description == "")
            {
                descriptionText.text = "설명 없음";
            }
            else
            {
                descriptionText.text = item.description;
            }
        }

        Debug.Log("[ItemInfoDisplay] 표시: " + item.itemName);
    }

    public void HideItem(SalvageItem item)
    {
        if (currentItem != item) return;

        Hide();
    }

    public void Hide()
    {
        currentItem = null;

        if (panelObject != null)
        {
            panelObject.SetActive(false);
        }
    }

    private string GetRarityName(int rarity)
    {
        if (rarity == 0)
        {
            return "일반";
        }

        if (rarity == 1)
        {
            return "희귀";
        }

        if (rarity == 2)
        {
            return "고급";
        }

        if (rarity == 3)
        {
            return "감성품";
        }

        if (rarity == 4)
        {
            return "재료";
        }

        return "알 수 없음";
    }
}