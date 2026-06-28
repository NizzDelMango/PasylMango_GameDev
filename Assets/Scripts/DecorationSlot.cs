using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class DecorationSlot : UdonSharpBehaviour
{
    public Transform snapPoint;
    public ComfortManager comfortManager;
    public FeedbackMessageDisplay feedbackDisplay;

    public bool occupied;

    public void OnTriggerEnter(Collider other)
    {
        if (occupied)
        {
            ShowFeedback("이 슬롯은 이미 사용 중입니다.");
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
            ShowFeedback("장식할 수 없는 아이템입니다: " + item.itemName);
            Debug.Log("[DecorationSlot] 장식 불가능한 아이템: " + item.itemName);
            return;
        }

        if (item.IsPlaced())
        {
            ShowFeedback("이미 배치된 아이템입니다: " + item.itemName);
            Debug.Log("[DecorationSlot] 이미 배치된 아이템: " + item.itemName);
            return;
        }

        if (item.IsInInventory())
        {
            ShowFeedback("가방에 들어간 아이템은 장식할 수 없습니다.");
            Debug.Log("[DecorationSlot] 가방에 들어간 아이템: " + item.itemName);
            return;
        }

        if (item.IsBeingSold())
        {
            ShowFeedback("이미 처리 중인 아이템은 장식할 수 없습니다.");
            Debug.Log("[DecorationSlot] 이미 처리 중인 아이템: " + item.itemName);
            return;
        }

        if (snapPoint == null)
        {
            ShowFeedback("장식 위치가 연결되지 않았습니다.");
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

        item.MarkPlaced();
        occupied = true;

        if (comfortManager != null)
        {
            comfortManager.AddComfort(item.comfortValue);
        }

        ShowFeedback("장식 완료: " + item.itemName);

        Debug.Log("[DecorationSlot] 배치 성공: " + item.itemName);
    }

    private void ShowFeedback(string message)
    {
        if (feedbackDisplay != null)
        {
            feedbackDisplay.ShowMessage(message);
        }
    }
}