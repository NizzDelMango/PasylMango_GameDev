using UdonSharp;
using UnityEngine;

public class InventorySellButton : UdonSharpBehaviour
{
    public SalvageInventory inventory;

    public override void Interact()
    {
        if (inventory == null)
        {
            Debug.Log("[InventorySellButton] inventory가 연결되지 않음");
            return;
        }

        inventory.SellAllStoredItems();
    }
}