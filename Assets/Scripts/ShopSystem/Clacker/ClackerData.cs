using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClackerData
{
    public string itemName;
    public int price;
    public int multiplier;
    public string description;
    public int sellPrice;
    public Color color;

    public bool isRandomColor;
    [SerializeReference] public List<ClackerData> nextClacker;
    public bool itemBought = false;

    public ClackerData() { }
    public ClackerData(string name, int p, int m, string desc, List<ClackerData> nextClack = default, bool isRandomColor = true, Color color = default, int sellPrice = -1)
    {
        itemName = name;
        price = p;
        multiplier = m;
        description = desc;
        this.isRandomColor = isRandomColor;
        this.color = color;
        this.sellPrice = sellPrice;
        this.nextClacker = nextClack;
    }

    public static ClackerData Convert(ClackerDataScriptable data)
    {
        return new ClackerData
        {
            itemName = data.itemName,
            price = data.price,
            multiplier = data.multiplier,
            description = data.description,
            nextClacker = data.nextClacker != null ? ConvertList(data.nextClacker) : default,
            isRandomColor = data.isRandomColor,
            sellPrice = data.sellPrice,
            color = data.color,
        };
    }
    public static List<ClackerData> ConvertList(List<ClackerDataScriptable> scriptables)
    {
        if (scriptables == null || scriptables.Count == 0)
            return null;

        List<ClackerData> lists = new List<ClackerData>();

        for (int i = 0; i < scriptables.Count; i++)
        {
            if (scriptables[i] != null)
                lists.Add(Convert(scriptables[i]));
        }
        return lists;
    }
}