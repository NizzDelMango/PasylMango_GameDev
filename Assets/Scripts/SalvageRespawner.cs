using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class SalvageRespawner : UdonSharpBehaviour
{
    public GameObject itemObject;
    public Transform spawnPoint;
    public float respawnDelay = 10f;

    private Vector3 cachedSpawnPosition;
    private Quaternion cachedSpawnRotation;
    private bool hasCachedSpawn;
    private bool respawnScheduled;

    private void Start()
    {
        CacheSpawnPoint();
    }

    private void CacheSpawnPoint()
    {
        if (spawnPoint != null)
        {
            cachedSpawnPosition = spawnPoint.position;
            cachedSpawnRotation = spawnPoint.rotation;
            hasCachedSpawn = true;

            Debug.Log("[SalvageRespawner] 스폰 위치 캐시 완료: " + spawnPoint.name);
            return;
        }

        if (itemObject != null)
        {
            cachedSpawnPosition = itemObject.transform.position;
            cachedSpawnRotation = itemObject.transform.rotation;
            hasCachedSpawn = true;

            Debug.Log("[SalvageRespawner] spawnPoint 없음 / itemObject 현재 위치를 스폰 위치로 캐시: " + itemObject.name);
            return;
        }

        hasCachedSpawn = false;
        Debug.Log("[SalvageRespawner] 스폰 위치 캐시 실패");
    }

    public void RespawnLater()
    {
        if (respawnScheduled)
        {
            Debug.Log("[SalvageRespawner] 이미 리스폰 예약됨");
            return;
        }

        if (respawnDelay < 0f)
        {
            respawnDelay = 0f;
        }

        respawnScheduled = true;

        Debug.Log("[SalvageRespawner] 리스폰 예약: " + respawnDelay + "초");

        SendCustomEventDelayedSeconds("RespawnNow", respawnDelay);
    }

    public void RespawnNow()
    {
        respawnScheduled = false;

        if (itemObject == null)
        {
            Debug.Log("[SalvageRespawner] itemObject가 연결되지 않음");
            return;
        }

        if (!hasCachedSpawn)
        {
            CacheSpawnPoint();
        }

        if (!hasCachedSpawn)
        {
            Debug.Log("[SalvageRespawner] 캐시된 스폰 위치가 없음");
            return;
        }

        itemObject.transform.position = cachedSpawnPosition;
        itemObject.transform.rotation = cachedSpawnRotation;

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