using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Skills/Skill Data")]
public class SkillDataScriptable :ScriptableObject
{
    public string itemName;
    public int price;
    public float multiplier;
    public SkillType skillType;
    public string description;
    public float priceTax;
    public float priceTaxInc;
    public List<SkillDataScriptable> nextSkill;
    public int level;
    public int levelMax;

}
