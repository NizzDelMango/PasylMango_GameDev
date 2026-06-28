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

    public SalvageRespawner respawner;

    [HideInInspector] public bool isPlaced;
    [HideInInspector] public bool isBeingSold;
    [HideInInspector] public bool isInInventory;
}