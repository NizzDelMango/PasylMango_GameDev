using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class TravelPoint : UdonSharpBehaviour
{
    public Transform targetPoint;
    public string travelName = "이동";

    public AreaTitleDisplay areaTitleDisplay;
    public string areaTitle;
    public string areaSubtitle;

    public AreaActivationManager areaActivationManager;
    public GameObject areaToActivateBeforeTravel;
    public bool deactivateAllAreasAfterTravel;

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

        if (deactivateAllAreasAfterTravel && areaToActivateBeforeTravel != null)
        {
            Debug.Log("[TravelPoint] deactivateAllAreasAfterTravel가 true이므로 areaToActivateBeforeTravel는 무시됨: " + travelName);
        }

        if (!deactivateAllAreasAfterTravel && areaActivationManager != null && areaToActivateBeforeTravel != null)
        {
            areaActivationManager.ActivateArea(areaToActivateBeforeTravel);
        }

        Debug.Log("[TravelPoint] 이동: " + travelName);

        localPlayer.TeleportTo(targetPoint.position, targetPoint.rotation);

        if (deactivateAllAreasAfterTravel && areaActivationManager != null)
        {
            areaActivationManager.DeactivateAllAreas();
        }

        if (areaTitleDisplay != null)
        {
            areaTitleDisplay.ShowAreaTitle(areaTitle, areaSubtitle);
        }
    }
}