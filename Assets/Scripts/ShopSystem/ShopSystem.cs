using System;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using UnityEngine;

public enum SkillType
{
    Multiply,
    Force,
    CapDistance
}

public enum ShopState
{
    clacker,
    skill
}
public class ShopSystem : MonoBehaviour // will continue here
{
    public static ShopSystem instance;
    #region "scriptable"
    public ClackerDataScriptable startingClackers;
    public SkillDataScriptable startingSkills;
    #endregion
    public static Dictionary<string, GameObject> itemObject = new Dictionary<string, GameObject>();
    public static Dictionary<string, ClackerData> clackerItemsBack = new Dictionary<string, ClackerData>();
    public static Dictionary<string, SkillData> skillItemsBack = new Dictionary<string, SkillData>();

    [SerializeField] GameObject itemPrefab;
    [SerializeField] GameObject skillPrefab;
    [SerializeField] GameObject ClackerShop;
    [SerializeField] GameObject content;
    public static bool clackerShopAllowed = true;
    bool shopOpen = true;

    Tween clackerShopTween;

    Vector2 clackerShopPosDefault;

    [SerializeField] float contentWidth,contentHeight;
    
    public ShopState shopState;

    public static Color[] colorChoices = {Color.cyan, Color.orange, Color.purple, Color.pink};

    public static bool initialized = false;

    void Awake()
    {
        if (instance == null)
            instance = this;

        shopState = ShopState.clacker;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clackerShopPosDefault = GetComponent<RectTransform>().anchoredPosition;

        if (!initialized)
        {
            LoadClackers();
            LoadSkills();
            initialized = true;
        }
        SetShopState("clacker");

    }
    void LoadClackers()
    {
        ClackerData data = ClackerData.Convert(startingClackers);
        clackerItemsBack.Add(data.itemName, data);
    }
    void LoadSkills()
    {
        SkillData data = SkillData.Convert(startingSkills);
        skillItemsBack.Add(data.itemName, data);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveClackerShop()
    {
        RectTransform ClaR = ClackerShop.GetComponent<RectTransform>();

        if (!clackerShopAllowed) return ;

        if (clackerShopTween.isAlive)
            clackerShopTween.Stop();
        if (shopOpen)
        {
            clackerShopTween = Tween.UIAnchoredPositionX(ClaR,clackerShopPosDefault.x,0.3f,Ease.OutSine);
            shopOpen = false;
        }
        else
        {
            clackerShopTween = Tween.UIAnchoredPositionX(ClaR,clackerShopPosDefault.x - ClaR.sizeDelta.x,0.3f,Ease.OutSine);
            shopOpen = true;
        }
    }

    public void AddItem(ClackerData clackData)
    {
        if (!clackerItemsBack.ContainsKey(clackData.itemName))
        {
            clackerItemsBack.Add(clackData.itemName, clackData);

        }
        else
        {
            Debug.LogWarning(clackData.itemName + " item already exist in shop panel");
        }
    }

    public void AddSkill(SkillData skillData)
    {
        if (!skillItemsBack.ContainsKey(name))
        {
            skillItemsBack.Add(skillData.itemName, skillData);
        
            // if (shopState == ShopState.skill)
            //     BringSkillItems();
        }
        else
        {
            Debug.LogWarning(name + " item already exist in shop panel");
        }
    }
    public void SetAsClackers()
    {
        shopState = ShopState.clacker;
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        itemObject.Clear();
        BringClackerItems();
    }
    public void SetAsSkills()
    {
        shopState = ShopState.skill;
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        itemObject.Clear();
        BringSkillItems();
    }

    public void BringSkillItems()
    {
        foreach (var skillData in skillItemsBack.Values)
        {
            GameObject item = Instantiate(skillPrefab, content.transform, false);
            item.GetComponent<SkillItem>().SetItem(skillData);
            itemObject.Add(skillData.itemName, item);
        }
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(contentWidth, itemObject.Count * contentHeight);
    }
    public void BringClackerItems()
    {
        var values = new List<ClackerData>(clackerItemsBack.Values);
        foreach (var clack in values)
        {
            GameObject item = Instantiate(itemPrefab, content.transform, false);
            item.GetComponent<ClackerItem>().SetItem(clack);
            itemObject.Add(clack.itemName, item);
        }
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(contentWidth, itemObject.Count * contentHeight);
    }

    public void AddClackerNow(ClackerData clack)
    {
        GameObject item = Instantiate(itemPrefab, content.transform, false);
        item.GetComponent<ClackerItem>().SetItem(clack);
        item.GetComponent<ClackerItem>().TweenItem();
        itemObject.Add(clack.itemName, item);
        clackerItemsBack.Add(clack.itemName, clack);
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(contentWidth, itemObject.Count * contentHeight);
        Tween.UIAnchoredPositionY(content.GetComponent<RectTransform>(),(itemObject.Count * contentHeight) - contentHeight - 100,0.3f,Ease.OutCirc);
    }
    public void AddSkillNow(SkillData skill)
    {
        GameObject item = Instantiate(skillPrefab, content.transform, false);
        item.GetComponent<SkillItem>().SetItem(skill);
        item.GetComponent<SkillItem>().TweenItem();
        itemObject.Add(skill.itemName, item);
        skillItemsBack.Add(skill.itemName, skill);
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(contentWidth, itemObject.Count * contentHeight);
        Tween.UIAnchoredPositionY(content.GetComponent<RectTransform>(),(itemObject.Count * contentHeight) - contentHeight - 100,0.3f,Ease.OutCirc);
    }
    public void SetShopState(string state)
    {
        switch (state.ToLower())
        {
            case "clacker":SetAsClackers();break;
            case "skill":SetAsSkills();break;

        }
    }
}
