using UdonSharp;
using UnityEngine;
using TMPro;

public class ItemInfoDisplay : UdonSharpBehaviour
{
    public GameObject panelObject;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI decorateText;

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
}