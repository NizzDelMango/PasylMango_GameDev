using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class SalvageItem : UdonSharpBehaviour
{
    public int itemId;
    public string itemName;
    public int sellPrice = 10;
    public bool canDecorate = true;

    [HideInInspector] public bool isPlaced;
}