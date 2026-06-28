using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class InventoryDepositBox : UdonSharpBehaviour
{
    public SalvageInventory inventory;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("[InventoryDepositBox] 무언가 들어옴: " + other.gameObject.name);

        if (inventory == null)
        {
            Debug.Log("[InventoryDepositBox] inventory가 연결되지 않음");
            return;
        }

        SalvageItem item = other.gameObject.GetComponent<SalvageItem>();

        if (item == null)
        {
            Debug.Log("[InventoryDepositBox] SalvageItem 아님");
            return;
        }

        if (item.isBeingSold)
        {
            Debug.Log("[InventoryDepositBox] 이미 처리 중인 아이템: " + item.itemName);
            return;
        }

        bool stored = inventory.TryStoreItem(item);

        if (!stored)
        {
            return;
        }

        VRC_Pickup pickup = other.gameObject.GetComponent<VRC_Pickup>();

        if (pickup != null)
        {
            pickup.Drop();
        }

        other.gameObject.SetActive(false);
    }
}