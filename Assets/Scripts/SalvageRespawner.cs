using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class SalvageRespawner : UdonSharpBehaviour
{
    public GameObject itemObject;
    public Transform spawnPoint;
    public float respawnDelay = 10f;

    public void RespawnLater()
    {
        if (respawnDelay < 0f)
        {
            respawnDelay = 0f;
        }

        Debug.Log("[SalvageRespawner] 리스폰 예약: " + respawnDelay + "초");

        SendCustomEventDelayedSeconds("RespawnNow", respawnDelay);
    }

    public void RespawnNow()
    {
        if (itemObject == null)
        {
            Debug.Log("[SalvageRespawner] itemObject가 연결되지 않음");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.Log("[SalvageRespawner] spawnPoint가 연결되지 않음");
            return;
        }

        itemObject.transform.position = spawnPoint.position;
        itemObject.transform.rotation = spawnPoint.rotation;

        Rigidbody rb = itemObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;
        }

        VRC_Pickup pickup = itemObject.GetComponent<VRC_Pickup>();

        if (pickup != null)
        {
            pickup.pickupable = true;
        }

        SalvageItem item = itemObject.GetComponent<SalvageItem>();

        if (item != null)
        {
            item.ResetState();
        }

        itemObject.SetActive(true);

        Debug.Log("[SalvageRespawner] 리스폰 완료: " + itemObject.name);
    }
}