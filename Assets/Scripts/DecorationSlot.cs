using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class DecorationSlot : UdonSharpBehaviour
{
    public Transform snapPoint;
    public ComfortManager comfortManager;
    public FeedbackMessageDisplay feedbackDisplay;

    public bool occupied;

    private GameObject placedItemObject;
    private SalvageItem placedItem;

    public void OnTriggerEnter(Collider other)
    {
        if (occupied)
        {
            ShowFeedback("이 슬롯은 이미 사용 중입니다. 슬롯을 상호작용하면 회수할 수 있습니다.");
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

    public override void Interact()
    {
        if (!occupied)
        {
            ShowFeedback("회수할 장식이 없습니다.");
            Debug.Log("[DecorationSlot] 회수할 장식 없음");
            return;
        }

        RemovePlacedItem();
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

        placedItemObject = itemObject;
        placedItem = item;
        occupied = true;

        if (comfortManager != null)
        {
            comfortManager.AddComfort(item.comfortValue);
        }

        ShowFeedback("장식 완료: " + item.itemName);

        Debug.Log("[DecorationSlot] 배치 성공: " + item.itemName);
    }

    private void RemovePlacedItem()
    {
        if (placedItemObject == null || placedItem == null)
        {
            placedItemObject = null;
            placedItem = null;
            occupied = false;

            ShowFeedback("장식 슬롯 상태를 초기화했습니다.");
            Debug.Log("[DecorationSlot] 배치 정보 없음 / 슬롯 초기화");
            return;
        }

        if (comfortManager != null)
        {
            comfortManager.RemoveComfort(placedItem.comfortValue);
        }

        placedItem.ResetState();

        placedItemObject.SetActive(true);

        Vector3 releasePosition = GetReleasePosition();
        Quaternion releaseRotation = GetReleaseRotation();

        placedItemObject.transform.position = releasePosition;
        placedItemObject.transform.rotation = releaseRotation;

        Rigidbody rb = placedItemObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;
        }

        VRC_Pickup pickup = placedItemObject.GetComponent<VRC_Pickup>();

        if (pickup != null)
        {
            pickup.pickupable = true;
        }

        ShowFeedback("장식 회수: " + placedItem.itemName);

        Debug.Log("[DecorationSlot] 장식 회수 완료: " + placedItem.itemName);

        placedItemObject = null;
        placedItem = null;
        occupied = false;
    }

    private Vector3 GetReleasePosition()
    {
        VRCPlayerApi localPlayer = Networking.LocalPlayer;

        if (localPlayer != null)
        {
            return localPlayer.GetPosition() + localPlayer.GetRotation() * Vector3.forward * 0.8f + Vector3.up * 0.6f;
        }

        if (snapPoint != null)
        {
            return snapPoint.position + snapPoint.forward * 0.8f + Vector3.up * 0.2f;
        }

        return transform.position + transform.forward * 0.8f + Vector3.up * 0.2f;
    }

    private Quaternion GetReleaseRotation()
    {
        VRCPlayerApi localPlayer = Networking.LocalPlayer;

        if (localPlayer != null)
        {
            return localPlayer.GetRotation();
        }

        if (snapPoint != null)
        {
            return snapPoint.rotation;
        }

        return transform.rotation;
    }

    private void ShowFeedback(string message)
    {
        if (feedbackDisplay != null)
        {
            feedbackDisplay.ShowMessage(message);
        }
    }
}