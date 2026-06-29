using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class PlayerHudFollower : UdonSharpBehaviour
{
    public Transform hudRoot;

    public float distanceFromHead = 1.2f;
    public float verticalOffset = -0.15f;
    public float horizontalOffset = 0f;

    public bool followHeadPitch = false;
    public bool smoothFollow = true;

    public float positionSmoothSpeed = 10f;
    public float rotationSmoothSpeed = 10f;

    private VRCPlayerApi localPlayer;
    private bool initialized;

    private void Start()
    {
        localPlayer = Networking.LocalPlayer;

        if (hudRoot == null)
        {
            hudRoot = transform;
        }

        if (localPlayer == null)
        {
            Debug.Log("[PlayerHudFollower] localPlayer를 찾을 수 없음");
            return;
        }

        initialized = true;

        UpdateHudTransform(true);

        Debug.Log("[PlayerHudFollower] HUD 추적 시작");
    }

    private void LateUpdate()
    {
        if (!initialized)
        {
            return;
        }

        UpdateHudTransform(false);
    }

    private void UpdateHudTransform(bool instant)
    {
        VRCPlayerApi.TrackingData headData = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);

        Vector3 headPosition = headData.position;
        Quaternion headRotation = headData.rotation;

        Vector3 forward = headRotation * Vector3.forward;
        Vector3 right = headRotation * Vector3.right;

        Vector3 targetPosition =
            headPosition
            + forward * distanceFromHead
            + right * horizontalOffset
            + Vector3.up * verticalOffset;

        Quaternion targetRotation;

        if (followHeadPitch)
        {
            targetRotation = Quaternion.LookRotation(forward, Vector3.up);
        }
        else
        {
            Vector3 flatForward = forward;
            flatForward.y = 0f;

            if (flatForward.sqrMagnitude < 0.001f)
            {
                flatForward = transform.forward;
            }

            targetRotation = Quaternion.LookRotation(flatForward.normalized, Vector3.up);
        }

        if (instant || !smoothFollow)
        {
            hudRoot.position = targetPosition;
            hudRoot.rotation = targetRotation;
            return;
        }

        hudRoot.position = Vector3.Lerp(
            hudRoot.position,
            targetPosition,
            Time.deltaTime * positionSmoothSpeed
        );

        hudRoot.rotation = Quaternion.Slerp(
            hudRoot.rotation,
            targetRotation,
            Time.deltaTime * rotationSmoothSpeed
        );
    }
}