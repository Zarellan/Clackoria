using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SkillData
{
    public string itemName;
    public int price;
    public float multiplier;
    public SkillType skillType;
    public string description;
    public float priceTax;
    public float priceTaxInc;
    [SerializeReference] public List<SkillData> nextSkill;
    public int level;
    public int levelMax;
    public bool itemBought = false;
    public SkillData() { }
    public SkillData(string name, int p, int m, float prTax, float prTIc, int levelMx,SkillType skill, string desc, List<SkillData> nextSkill = default)
    {
        itemName = name;
        price = p;
        multiplier = m;
        skillType = skill;
        description = desc;
        priceTax = prTax;
        priceTaxInc = prTIc;
        level = 0;
        levelMax = levelMx;
        this.nextSkill = nextSkill;
    }

    public static SkillData Convert(SkillDataScriptable data)
    {
        return new SkillData
        {
            itemName = data.itemName,
            price = data.price,
            multiplier = data.multiplier,
            priceTax = data.priceTax,
            priceTaxInc = data.priceTaxInc,
            description = data.description,
            level = 0,
            levelMax = data.levelMax,
            skillType = data.skillType,
            nextSkill = data.nextSkill != null ? ConvertList(data.nextSkill) : default,
        };
    }
    public static List<SkillData> ConvertList(List<SkillDataScriptable> scriptables)
    {
        if (scriptables == null || scriptables.Count == 0)
            return null;

        List<SkillData> lists = new List<SkillData>();

        for (int i = 0; i < scriptables.Count; i++)
        {
            if (scriptables[i] != null)
                lists.Add(Convert(scriptables[i]));
        }
        return lists;
    }
}