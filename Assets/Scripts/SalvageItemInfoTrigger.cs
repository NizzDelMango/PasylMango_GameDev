using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class SalvageItemInfoTrigger : UdonSharpBehaviour
{
    public SalvageItem item;
    public ItemInfoDisplay display;

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (!player.isLocal) return;

        if (item == null)
        {
            Debug.Log("[SalvageItemInfoTrigger] item이 연결되지 않음");
            return;
        }

        if (display == null)
        {
            Debug.Log("[SalvageItemInfoTrigger] display가 연결되지 않음");
            return;
        }

        if (item.isPlaced)
        {
            return;
        }

        display.ShowItem(item);
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (!player.isLocal) return;

        if (display == null) return;
        if (item == null) return;

        display.HideItem(item);
    }
}