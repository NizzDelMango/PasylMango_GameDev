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

    public string lastStoreFailMessage;

    private SalvageItem[] storedItems;
    private int itemCount;
    private int totalSellValue;

    private void Start()
    {
        if (maxStoredItems <= 0)
        {
            maxStoredItems = 1;
        }

        if (capacity < 0)
        {
            capacity = 0;
        }

        if (capacity > maxStoredItems)
        {
            capacity = maxStoredItems;
        }

        storedItems = new SalvageItem[maxStoredItems];

        RefreshUI();
    }

    public bool TryStoreItem(SalvageItem item)
    {
        lastStoreFailMessage = "";

        if (maxStoredItems <= 0)
        {
            maxStoredItems = 1;
        }

        if (storedItems == null)
        {
            storedItems = new SalvageItem[maxStoredItems];
        }

        if (item == null)
        {
            lastStoreFailMessage = "보관할 아이템을 찾을 수 없습니다.";
            Debug.Log("[SalvageInventory] item이 null");
            return false;
        }

        if (itemCount >= capacity)
        {
            lastStoreFailMessage = "가방이 가득 찼습니다.";
            Debug.Log("[SalvageInventory] 가방이 가득 참");
            return false;
        }

        if (item.IsPlaced())
        {
            lastStoreFailMessage = "장식된 아이템은 보관할 수 없습니다.";
            Debug.Log("[SalvageInventory] 장식된 아이템은 보관 불가: " + item.itemName);
            return false;
        }

        if (item.IsInInventory())
        {
            lastStoreFailMessage = "이미 가방에 들어간 아이템입니다.";
            Debug.Log("[SalvageInventory] 이미 가방에 들어간 아이템: " + item.itemName);
            return false;
        }

        if (item.IsBeingSold())
        {
            lastStoreFailMessage = "이미 처리 중인 아이템입니다.";
            Debug.Log("[SalvageInventory] 이미 처리 중인 아이템: " + item.itemName);
            return false;
        }

        int slotIndex = FindEmptySlot();

        if (slotIndex < 0)
        {
            lastStoreFailMessage = "빈 가방 슬롯을 찾을 수 없습니다.";
            Debug.Log("[SalvageInventory] 빈 슬롯을 찾지 못함");
            return false;
        }

        storedItems[slotIndex] = item;
        item.MarkStored();

        itemCount += 1;
        totalSellValue += item.sellPrice;

        Debug.Log("[SalvageInventory] 보관 성공: " + item.itemName);

        RefreshUI();

        return true;
    }

    public string GetLastStoreFailMessage()
    {
        return lastStoreFailMessage;
    }

    public bool HasStoredItems()
    {
        return itemCount > 0;
    }

    public bool SellAllStoredItems()
    {
        if (storedItems == null)
        {
            Debug.Log("[SalvageInventory] 판매 실패 / storedItems가 초기화되지 않음");
            return false;
        }

        if (itemCount <= 0)
        {
            Debug.Log("[SalvageInventory] 판매할 아이템 없음");
            return false;
        }

        if (moneyManager == null)
        {
            Debug.Log("[SalvageInventory] 판매 실패 / moneyManager가 연결되지 않음");
            return false;
        }

        if (totalSellValue <= 0)
        {
            Debug.Log("[SalvageInventory] 판매 실패 / totalSellValue <= 0: " + totalSellValue);
            return false;
        }

        moneyManager.AddMoney(totalSellValue);

        Debug.Log("[SalvageInventory] 가방 아이템 판매 / 총액: " + totalSellValue);

        for (int i = 0; i < storedItems.Length; i++)
        {
            SalvageItem item = storedItems[i];

            if (item == null) continue;

            item.MarkSelling();

            if (item.respawner != null)
            {
                item.respawner.RespawnLater();
            }
            else
            {
                item.ResetState();
                Debug.Log("[SalvageInventory] respawner 없는 아이템: " + item.itemName);
            }

            storedItems[i] = null;
        }

        itemCount = 0;
        totalSellValue = 0;

        RefreshUI();

        return true;
    }

    public void IncreaseCapacity(int amount)
    {
        if (amount <= 0)
        {
            Debug.Log("[SalvageInventory] IncreaseCapacity 실패 / amount <= 0: " + amount);
            return;
        }

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
        if (storedItems == null)
        {
            return -1;
        }

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

        if (storedItems == null)
        {
            return text + "\n- 오류: 가방이 초기화되지 않음";
        }

        for (int i = 0; i < storedItems.Length; i++)
        {
            SalvageItem item = storedItems[i];

            if (item == null) continue;

            text += "\n- " + item.itemName;
        }

        return text;
    }
}