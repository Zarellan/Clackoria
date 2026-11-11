using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Clacker", menuName = "Clacker/Clacker Data")]
public class ClackerDataScriptable : ScriptableObject
{
    public string itemName;
    public int price;
    public int multiplier;
    public string description;

    public Color color;
    public int sellPrice;
    public bool isRandomColor;
    public List<ClackerDataScriptable> nextClacker;
    public bool itemBought = false;

}