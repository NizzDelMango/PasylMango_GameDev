using UdonSharp;
using UnityEngine;
using TMPro;

public class SalvageInventory : UdonSharpBehaviour
{
    public MoneyManager moneyManager;

    public int capacity = 3;
    public int maxStoredItems = 20;

    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI inventoryListText;

    private SalvageItem[] storedItems;
    private int itemCount;
    private int totalSellValue;

    private void Start()
    {
        storedItems = new SalvageItem[maxStoredItems];
        RefreshUI();
    }

    public bool TryStoreItem(SalvageItem item)
    {
        if (item == null)
        {
            Debug.Log("[SalvageInventory] item이 null");
            return false;
        }

        if (itemCount >= capacity)
        {
            Debug.Log("[SalvageInventory] 가방이 가득 참");
            return false;
        }

        if (item.isPlaced)
        {
            Debug.Log("[SalvageInventory] 장식된 아이템은 보관 불가: " + item.itemName);
            return false;
        }

        if (item.isInInventory)
        {
            Debug.Log("[SalvageInventory] 이미 가방에 들어간 아이템: " + item.itemName);
            return false;
        }

        int slotIndex = FindEmptySlot();

        if (slotIndex < 0)
        {
            Debug.Log("[SalvageInventory] 빈 슬롯을 찾지 못함");
            return false;
        }

        storedItems[slotIndex] = item;

        item.isInInventory = true;
        item.isBeingSold = true;

        itemCount += 1;
        totalSellValue += item.sellPrice;

        Debug.Log("[SalvageInventory] 보관 성공: " + item.itemName);

        RefreshUI();

        return true;
    }

    public void SellAllStoredItems()
    {
        if (itemCount <= 0)
        {
            Debug.Log("[SalvageInventory] 판매할 아이템 없음");
            return;
        }

        if (moneyManager == null)
        {
            Debug.Log("[SalvageInventory] moneyManager가 연결되지 않음");
            return;
        }

        moneyManager.AddMoney(totalSellValue);

        Debug.Log("[SalvageInventory] 가방 아이템 판매 / 총액: " + totalSellValue);

        for (int i = 0; i < storedItems.Length; i++)
        {
            SalvageItem item = storedItems[i];

            if (item == null) continue;

            if (item.respawner != null)
            {
                item.respawner.RespawnLater();
            }
            else
            {
                item.isInInventory = false;
                item.isBeingSold = false;

                Debug.Log("[SalvageInventory] respawner 없는 아이템: " + item.itemName);
            }

            storedItems[i] = null;
        }

        itemCount = 0;
        totalSellValue = 0;

        RefreshUI();
    }

    public void IncreaseCapacity(int amount)
    {
        capacity += amount;

        if (capacity > maxStoredItems)
        {
            capacity = maxStoredItems;
        }

        Debug.Log("[SalvageInventory] 가방 용량 증가 / 현재 용량: " + capacity);

        RefreshUI();
    }

    private int FindEmptySlot()
    {
        for (int i = 0; i < storedItems.Length; i++)
        {
            if (storedItems[i] == null)
            {
                return i;
            }
        }

        return -1;
    }

    private void RefreshUI()
    {
        if (inventoryText != null)
        {
            inventoryText.text = "가방: " + itemCount + " / " + capacity + "\n예상 판매가: " + totalSellValue;
        }

        if (inventoryListText != null)
        {
            inventoryListText.text = GetInventoryListText();
        }
    }

    private string GetInventoryListText()
    {
        if (itemCount <= 0)
        {
            return "보관 중:\n- 없음";
        }

        string text = "보관 중:";

        for (int i = 0; i < storedItems.Length; i++)
        {
            SalvageItem item = storedItems[i];

            if (item == null) continue;

            text += "\n- " + item.itemName;
        }

        return text;
    }
}