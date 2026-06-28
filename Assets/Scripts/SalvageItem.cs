using UdonSharp;
using UnityEngine;

public class SalvageItem : UdonSharpBehaviour
{
    public int itemId;
    public string itemName;
    public int sellPrice = 10;
    public bool canDecorate = true;

    public int rarity = 0;
    public int comfortValue = 1;

    public string description;

    public SalvageRespawner respawner;

    [HideInInspector] public bool isPlaced;
    [HideInInspector] public bool isBeingSold;
    [HideInInspector] public bool isInInventory;

    public bool IsPlaced()
    {
        return isPlaced;
    }

    public bool IsBeingSold()
    {
        return isBeingSold;
    }

    public bool IsInInventory()
    {
        return isInInventory;
    }

    public bool CanBeStored()
    {
        return !isPlaced && !isInInventory && !isBeingSold;
    }

    public bool CanBeSoldDirectly()
    {
        return !isPlaced && !isInInventory && !isBeingSold;
    }

    public bool CanBeDecoratedNow()
    {
        return canDecorate && !isPlaced && !isInInventory && !isBeingSold;
    }

    public void MarkPlaced()
    {
        isPlaced = true;
        isBeingSold = false;
        isInInventory = false;

        Debug.Log("[SalvageItem] MarkPlaced: " + itemName);
    }

    public void MarkStored()
    {
        isPlaced = false;
        isBeingSold = true;
        isInInventory = true;

        Debug.Log("[SalvageItem] MarkStored: " + itemName);
    }

    public void MarkSelling()
    {
        isPlaced = false;
        isBeingSold = true;
        isInInventory = false;

        Debug.Log("[SalvageItem] MarkSelling: " + itemName);
    }

    public void ResetState()
    {
        isPlaced = false;
        isBeingSold = false;
        isInInventory = false;

        Debug.Log("[SalvageItem] ResetState: " + itemName);
    }
}