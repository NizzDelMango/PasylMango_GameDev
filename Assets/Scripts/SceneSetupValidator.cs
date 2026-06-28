using UdonSharp;
using UnityEngine;

public class SceneSetupValidator : UdonSharpBehaviour
{
    [Header("Core Managers")]
    public MoneyManager moneyManager;
    public ComfortManager comfortManager;
    public SalvageInventory inventory;
    public AreaActivationManager areaActivationManager;

    [Header("UI")]
    public FeedbackMessageDisplay feedbackDisplay;
    public ItemInfoDisplay itemInfoDisplay;
    public AreaTitleDisplay areaTitleDisplay;

    [Header("Gameplay Objects")]
    public SalvageItem[] salvageItems;
    public SalvageRespawner[] respawners;
    public DecorationSlot[] decorationSlots;
    public TravelPoint[] travelPoints;

    [Header("Buttons")]
    public UpgradeButton[] upgradeButtons;
    public InventoryCapacityUpgradeButton[] inventoryCapacityUpgradeButtons;
    public ComfortUnlockButton[] comfortUnlockButtons;
    public ExplorationAreaUnlockButton[] explorationAreaUnlockButtons;

    [Header("Options")]
    public bool validateOnStart = true;

    private int errorCount;
    private int warningCount;

    private void Start()
    {
        if (validateOnStart)
        {
            RunValidation();
        }
    }

    public override void Interact()
    {
        RunValidation();
    }

    public void RunValidation()
    {
        errorCount = 0;
        warningCount = 0;

        Debug.Log("[SceneSetupValidator] 씬 검증 시작");

        ValidateCoreManagers();
        ValidateUI();
        ValidateSalvageItems();
        ValidateRespawners();
        ValidateDecorationSlots();
        ValidateTravelPoints();
        ValidateUpgradeButtons();
        ValidateInventoryCapacityUpgradeButtons();
        ValidateComfortUnlockButtons();
        ValidateExplorationAreaUnlockButtons();

        Debug.Log("[SceneSetupValidator] 씬 검증 완료 / 오류: " + errorCount + " / 경고: " + warningCount);
    }

    private void ValidateCoreManagers()
    {
        if (moneyManager == null)
        {
            LogError("MoneyManager가 연결되지 않았습니다.");
        }
        else if (moneyManager.moneyText == null)
        {
            LogWarning("MoneyManager.moneyText가 연결되지 않았습니다.");
        }

        if (comfortManager == null)
        {
            LogError("ComfortManager가 연결되지 않았습니다.");
        }
        else if (comfortManager.comfortText == null)
        {
            LogWarning("ComfortManager.comfortText가 연결되지 않았습니다.");
        }

        if (inventory == null)
        {
            LogError("SalvageInventory가 연결되지 않았습니다.");
        }
        else
        {
            if (inventory.moneyManager == null)
            {
                LogError("SalvageInventory.moneyManager가 연결되지 않았습니다.");
            }

            if (inventory.capacity < 0)
            {
                LogError("SalvageInventory.capacity가 0보다 작습니다.");
            }

            if (inventory.maxStoredItems <= 0)
            {
                LogError("SalvageInventory.maxStoredItems가 0 이하입니다.");
            }

            if (inventory.capacity > inventory.maxStoredItems)
            {
                LogWarning("SalvageInventory.capacity가 maxStoredItems보다 큽니다.");
            }

            if (inventory.inventoryText == null)
            {
                LogWarning("SalvageInventory.inventoryText가 연결되지 않았습니다.");
            }

            if (inventory.inventoryListText == null)
            {
                LogWarning("SalvageInventory.inventoryListText가 연결되지 않았습니다.");
            }
        }

        if (areaActivationManager == null)
        {
            LogWarning("AreaActivationManager가 연결되지 않았습니다.");
        }
        else
        {
            if (areaActivationManager.explorationAreas == null)
            {
                LogError("AreaActivationManager.explorationAreas가 null입니다.");
            }
            else if (areaActivationManager.explorationAreas.Length <= 0)
            {
                LogWarning("AreaActivationManager.explorationAreas가 비어 있습니다.");
            }
            else
            {
                for (int i = 0; i < areaActivationManager.explorationAreas.Length; i++)
                {
                    if (areaActivationManager.explorationAreas[i] == null)
                    {
                        LogError("AreaActivationManager.explorationAreas[" + i + "]가 null입니다.");
                    }
                }
            }
        }
    }

    private void ValidateUI()
    {
        if (feedbackDisplay == null)
        {
            LogWarning("FeedbackMessageDisplay가 연결되지 않았습니다.");
        }
        else
        {
            if (feedbackDisplay.panelObject == null)
            {
                LogError("FeedbackMessageDisplay.panelObject가 연결되지 않았습니다.");
            }

            if (feedbackDisplay.messageText == null)
            {
                LogError("FeedbackMessageDisplay.messageText가 연결되지 않았습니다.");
            }

            if (feedbackDisplay.hideDelay < 0f)
            {
                LogWarning("FeedbackMessageDisplay.hideDelay가 0보다 작습니다.");
            }
        }

        if (itemInfoDisplay == null)
        {
            LogWarning("ItemInfoDisplay가 연결되지 않았습니다.");
        }
        else
        {
            if (itemInfoDisplay.panelObject == null)
            {
                LogError("ItemInfoDisplay.panelObject가 연결되지 않았습니다.");
            }

            if (itemInfoDisplay.nameText == null)
            {
                LogWarning("ItemInfoDisplay.nameText가 연결되지 않았습니다.");
            }

            if (itemInfoDisplay.priceText == null)
            {
                LogWarning("ItemInfoDisplay.priceText가 연결되지 않았습니다.");
            }

            if (itemInfoDisplay.decorateText == null)
            {
                LogWarning("ItemInfoDisplay.decorateText가 연결되지 않았습니다.");
            }

            if (itemInfoDisplay.rarityText == null)
            {
                LogWarning("ItemInfoDisplay.rarityText가 연결되지 않았습니다.");
            }

            if (itemInfoDisplay.descriptionText == null)
            {
                LogWarning("ItemInfoDisplay.descriptionText가 연결되지 않았습니다.");
            }
        }

        if (areaTitleDisplay == null)
        {
            LogWarning("AreaTitleDisplay가 연결되지 않았습니다.");
        }
    }

    private void ValidateSalvageItems()
    {
        if (salvageItems == null || salvageItems.Length <= 0)
        {
            LogWarning("salvageItems 배열이 비어 있습니다.");
            return;
        }

        for (int i = 0; i < salvageItems.Length; i++)
        {
            SalvageItem item = salvageItems[i];

            if (item == null)
            {
                LogError("salvageItems[" + i + "]가 null입니다.");
                continue;
            }

            if (item.itemId <= 0)
            {
                LogWarning("SalvageItem itemId가 0 이하입니다: " + item.gameObject.name);
            }

            if (item.itemName == null || item.itemName == "")
            {
                LogWarning("SalvageItem itemName이 비어 있습니다: " + item.gameObject.name);
            }

            if (item.sellPrice < 0)
            {
                LogError("SalvageItem sellPrice가 0보다 작습니다: " + item.itemName);
            }

            if (item.comfortValue < 0)
            {
                LogWarning("SalvageItem comfortValue가 0보다 작습니다: " + item.itemName);
            }

            if (item.respawner == null)
            {
                LogWarning("SalvageItem respawner가 연결되지 않았습니다: " + item.itemName);
            }
        }
    }

    private void ValidateRespawners()
    {
        if (respawners == null || respawners.Length <= 0)
        {
            LogWarning("respawners 배열이 비어 있습니다.");
            return;
        }

        for (int i = 0; i < respawners.Length; i++)
        {
            SalvageRespawner respawner = respawners[i];

            if (respawner == null)
            {
                LogError("respawners[" + i + "]가 null입니다.");
                continue;
            }

            if (respawner.itemObject == null)
            {
                LogError("SalvageRespawner.itemObject가 연결되지 않았습니다: " + respawner.gameObject.name);
            }

            if (respawner.spawnPoint == null)
            {
                LogError("SalvageRespawner.spawnPoint가 연결되지 않았습니다: " + respawner.gameObject.name);
            }

            if (respawner.respawnDelay < 0f)
            {
                LogWarning("SalvageRespawner.respawnDelay가 0보다 작습니다: " + respawner.gameObject.name);
            }
        }
    }

    private void ValidateDecorationSlots()
    {
        if (decorationSlots == null || decorationSlots.Length <= 0)
        {
            LogWarning("decorationSlots 배열이 비어 있습니다.");
            return;
        }

        for (int i = 0; i < decorationSlots.Length; i++)
        {
            DecorationSlot slot = decorationSlots[i];

            if (slot == null)
            {
                LogError("decorationSlots[" + i + "]가 null입니다.");
                continue;
            }

            if (slot.snapPoint == null)
            {
                LogError("DecorationSlot.snapPoint가 연결되지 않았습니다: " + slot.gameObject.name);
            }

            if (slot.comfortManager == null)
            {
                LogWarning("DecorationSlot.comfortManager가 연결되지 않았습니다: " + slot.gameObject.name);
            }

            if (slot.feedbackDisplay == null)
            {
                LogWarning("DecorationSlot.feedbackDisplay가 연결되지 않았습니다: " + slot.gameObject.name);
            }
        }
    }

    private void ValidateTravelPoints()
    {
        if (travelPoints == null || travelPoints.Length <= 0)
        {
            LogWarning("travelPoints 배열이 비어 있습니다.");
            return;
        }

        for (int i = 0; i < travelPoints.Length; i++)
        {
            TravelPoint travelPoint = travelPoints[i];

            if (travelPoint == null)
            {
                LogError("travelPoints[" + i + "]가 null입니다.");
                continue;
            }

            if (travelPoint.targetPoint == null)
            {
                LogError("TravelPoint.targetPoint가 연결되지 않았습니다: " + travelPoint.gameObject.name);
            }

            if (travelPoint.areaTitleDisplay == null)
            {
                LogWarning("TravelPoint.areaTitleDisplay가 연결되지 않았습니다: " + travelPoint.gameObject.name);
            }

            if (travelPoint.areaActivationManager == null)
            {
                LogWarning("TravelPoint.areaActivationManager가 연결되지 않았습니다: " + travelPoint.gameObject.name);
            }

            if (travelPoint.deactivateAllAreasAfterTravel && travelPoint.areaToActivateBeforeTravel != null)
            {
                LogWarning("TravelPoint가 deactivateAllAreasAfterTravel와 areaToActivateBeforeTravel를 동시에 사용합니다: " + travelPoint.gameObject.name);
            }

            if (!travelPoint.deactivateAllAreasAfterTravel && travelPoint.areaActivationManager != null && travelPoint.areaToActivateBeforeTravel == null)
            {
                LogWarning("TravelPoint가 지역 활성화 매니저는 있지만 활성화할 지역이 없습니다: " + travelPoint.gameObject.name);
            }
        }
    }

    private void ValidateUpgradeButtons()
    {
        if (upgradeButtons == null) return;

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            UpgradeButton button = upgradeButtons[i];

            if (button == null)
            {
                LogError("upgradeButtons[" + i + "]가 null입니다.");
                continue;
            }

            if (button.moneyManager == null)
            {
                LogError("UpgradeButton.moneyManager가 연결되지 않았습니다: " + button.gameObject.name);
            }

            if (button.price <= 0)
            {
                LogError("UpgradeButton.price가 0 이하입니다: " + button.gameObject.name);
            }

            if (button.objectToUnlock == null)
            {
                LogError("UpgradeButton.objectToUnlock이 연결되지 않았습니다: " + button.gameObject.name);
            }

            if (button.feedbackDisplay == null)
            {
                LogWarning("UpgradeButton.feedbackDisplay가 연결되지 않았습니다: " + button.gameObject.name);
            }
        }
    }

    private void ValidateInventoryCapacityUpgradeButtons()
    {
        if (inventoryCapacityUpgradeButtons == null) return;

        for (int i = 0; i < inventoryCapacityUpgradeButtons.Length; i++)
        {
            InventoryCapacityUpgradeButton button = inventoryCapacityUpgradeButtons[i];

            if (button == null)
            {
                LogError("inventoryCapacityUpgradeButtons[" + i + "]가 null입니다.");
                continue;
            }

            if (button.moneyManager == null)
            {
                LogError("InventoryCapacityUpgradeButton.moneyManager가 연결되지 않았습니다: " + button.gameObject.name);
            }

            if (button.inventory == null)
            {
                LogError("InventoryCapacityUpgradeButton.inventory가 연결되지 않았습니다: " + button.gameObject.name);
            }

            if (button.price <= 0)
            {
                LogError("InventoryCapacityUpgradeButton.price가 0 이하입니다: " + button.gameObject.name);
            }

            if (button.capacityIncrease <= 0)
            {
                LogError("InventoryCapacityUpgradeButton.capacityIncrease가 0 이하입니다: " + button.gameObject.name);
            }

            if (button.feedbackDisplay == null)
            {
                LogWarning("InventoryCapacityUpgradeButton.feedbackDisplay가 연결되지 않았습니다: " + button.gameObject.name);
            }
        }
    }

    private void ValidateComfortUnlockButtons()
    {
        if (comfortUnlockButtons == null) return;

        for (int i = 0; i < comfortUnlockButtons.Length; i++)
        {
            ComfortUnlockButton button = comfortUnlockButtons[i];

            if (button == null)
            {
                LogError("comfortUnlockButtons[" + i + "]가 null입니다.");
                continue;
            }

            if (button.comfortManager == null)
            {
                LogError("ComfortUnlockButton.comfortManager가 연결되지 않았습니다: " + button.gameObject.name);
            }

            if (button.requiredComfort < 0)
            {
                LogError("ComfortUnlockButton.requiredComfort가 0보다 작습니다: " + button.gameObject.name);
            }

            if (button.objectToUnlock == null)
            {
                LogError("ComfortUnlockButton.objectToUnlock이 연결되지 않았습니다: " + button.gameObject.name);
            }

            if (button.feedbackDisplay == null)
            {
                LogWarning("ComfortUnlockButton.feedbackDisplay가 연결되지 않았습니다: " + button.gameObject.name);
            }
        }
    }

    private void ValidateExplorationAreaUnlockButtons()
    {
        if (explorationAreaUnlockButtons == null) return;

        for (int i = 0; i < explorationAreaUnlockButtons.Length; i++)
        {
            ExplorationAreaUnlockButton button = explorationAreaUnlockButtons[i];

            if (button == null)
            {
                LogError("explorationAreaUnlockButtons[" + i + "]가 null입니다.");
                continue;
            }

            if (button.objectToUnlock == null)
            {
                LogError("ExplorationAreaUnlockButton.objectToUnlock이 연결되지 않았습니다: " + button.gameObject.name);
            }

            if (button.requireMoney)
            {
                if (button.moneyManager == null)
                {
                    LogError("ExplorationAreaUnlockButton.moneyManager가 연결되지 않았습니다: " + button.gameObject.name);
                }

                if (button.price <= 0)
                {
                    LogError("ExplorationAreaUnlockButton.price가 0 이하입니다: " + button.gameObject.name);
                }
            }

            if (button.requireComfort)
            {
                if (button.comfortManager == null)
                {
                    LogError("ExplorationAreaUnlockButton.comfortManager가 연결되지 않았습니다: " + button.gameObject.name);
                }

                if (button.requiredComfort < 0)
                {
                    LogError("ExplorationAreaUnlockButton.requiredComfort가 0보다 작습니다: " + button.gameObject.name);
                }
            }

            if (button.feedbackDisplay == null)
            {
                LogWarning("ExplorationAreaUnlockButton.feedbackDisplay가 연결되지 않았습니다: " + button.gameObject.name);
            }
        }
    }

    private void LogError(string message)
    {
        errorCount += 1;
        Debug.Log("[SceneSetupValidator][ERROR] " + message);
    }

    private void LogWarning(string message)
    {
        warningCount += 1;
        Debug.Log("[SceneSetupValidator][WARNING] " + message);
    }
}