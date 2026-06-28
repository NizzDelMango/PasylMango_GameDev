using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class DecorationSlot : UdonSharpBehaviour
{
    public Transform snapPoint;
    public ComfortManager comfortManager;

    public bool occupied;

    public void OnTriggerEnter(Collider other)
    {
        if (occupied)
        {
            Debug.Log("[DecorationSlot] 이미 사용 중인 슬롯");
            return;
        }

        SalvageItem item = other.gameObject.GetComponent<SalvageItem>();

        if (item == null)
        {
            Debug.Log("[DecorationSlot] SalvageItem 아님: " + other.gameObject.name);
            return;
        }

        if (!item.canDecorate)
        {
            Debug.Log("[DecorationSlot] 장식 불가능한 아이템: " + item.itemName);
            return;
        }

        if (item.isPlaced)
        {
            Debug.Log("[DecorationSlot] 이미 배치된 아이템: " + item.itemName);
            return;
        }

        if (snapPoint == null)
        {
            Debug.Log("[DecorationSlot] snapPoint가 연결되지 않음");
            return;
        }

        PlaceItem(other.gameObject, item);
    }

    private void PlaceItem(GameObject itemObject, SalvageItem item)
    {
        VRC_Pickup pickup = itemObject.GetComponent<VRC_Pickup>();

        if (pickup != null)
        {
            pickup.Drop();
            pickup.pickupable = false;
        }

        Rigidbody rb = itemObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        itemObject.transform.position = snapPoint.position;
        itemObject.transform.rotation = snapPoint.rotation;

        item.isPlaced = true;
        occupied = true;

        if (comfortManager != null)
        {
            comfortManager.AddComfort(item.comfortValue);
        }

        Debug.Log("[DecorationSlot] 배치 성공: " + item.itemName);
    }
}