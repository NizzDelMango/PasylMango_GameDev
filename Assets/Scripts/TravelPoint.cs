using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class TravelPoint : UdonSharpBehaviour
{
    public Transform targetPoint;
    public string travelName = "이동";

    public override void Interact()
    {
        if (targetPoint == null)
        {
            Debug.Log("[TravelPoint] targetPoint가 연결되지 않음: " + travelName);
            return;
        }

        VRCPlayerApi localPlayer = Networking.LocalPlayer;

        if (localPlayer == null)
        {
            Debug.Log("[TravelPoint] localPlayer를 찾을 수 없음");
            return;
        }

        Debug.Log("[TravelPoint] 이동: " + travelName);

        localPlayer.TeleportTo(targetPoint.position, targetPoint.rotation);
    }
}