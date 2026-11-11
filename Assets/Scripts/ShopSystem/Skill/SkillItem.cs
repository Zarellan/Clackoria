using System.Collections.Generic;
using PrimeTween;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{
    [SerializeField] GameObject image;
    [SerializeField] GameObject titleText;
    [SerializeField] GameObject priceText;
    [SerializeField] GameObject levelText;
    [SerializeField] GameObject levelMaxText;

    [SerializeField] GameObject visualPanel;
    [SerializeField] GameObject visualPanelHelp;
    [SerializeField] Sprite panel,panelWhite;
    Tween imageTween, rotationImageTween;
    public List<SkillData> nextSkill;

    AudioSource audi;
    SkillData skillMemory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audi = GetComponent<AudioSource>();
        audi.clip = Resources.Load<AudioClip>("Audio/money purchase");
    }
    public void TweenItem()
    {
        Tween.ScaleX(GetComponent<RectTransform>(),startValue:0f,endValue:1f,0.3f,Ease.OutCirc);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Print()
    {
        
    }
    // string name, int value, int multiply, float prTx, float prTxInc, int levelMx,SkillType skill, string desc
    public void SetItem(SkillData skillData)
    {
        // itemName = skillData.itemName;
        // price = skillData.price;
        // multiplier = skillData.multiplier;
        // description = skillData.description;
        // priceTax = skillData.priceTax;
        // priceTaxInc = skillData.priceTaxInc;
        // level = skillData.level;
        // levelMax = skillData.levelMax;
        skillMemory = skillData;
        image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/SkillItem/"+skillMemory.itemName);
        titleText.GetComponent<TMP_Text>().text = skillData.itemName;
        priceText.GetComponent<TMP_Text>().text = $"$ {FormatNumber.Format(skillData.price)}";
        levelText.GetComponent<TMP_Text>().text = $"lvl: {skillData.level}";
        levelMaxText.GetComponent<TMP_Text>().text = $"lvl: {skillData.levelMax}";
        // skillType = skillData.skillType;
        // nextSkill = skillData.nextSkill;
        // itemBought = skillData.itemBought;
    }

    public void BuyItem()
    {
        if (MarbleManager.instance.money >= skillMemory.price && skillMemory.levelMax > skillMemory.level)
        {
            AddPower();
            MarbleManager.instance.money -= skillMemory.price;
            ScaleMainPanel();
            skillMemory.price = Mathf.FloorToInt(skillMemory.price * skillMemory.priceTax);
            skillMemory.priceTax += skillMemory.priceTaxInc;
            skillMemory.level++;
            audi.pitch = Random.Range(0.97f,1.03f);
            audi.Play();
            ScaleMainPanel();
            Tween.Color(visualPanelHelp.GetComponent<Image>(),new Color(1,1,1,255.0f/144.0f),new Color(1,1,1,0),0.3f,Ease.OutCirc);

            if (skillMemory.nextSkill != null && skillMemory.nextSkill.Count > 0  && !skillMemory.itemBought)
            {
                for (int i = 0;i < skillMemory.nextSkill.Count;i++)
                {
                    ShopSystem.instance.AddSkillNow(skillMemory.nextSkill[i]);
                    
                }
                skillMemory.itemBought = true;
                ShopSystem.skillItemsBack[skillMemory.itemName].itemBought = true;
            }



            priceText.GetComponent<TMP_Text>().text = $"$ {FormatNumber.Format(skillMemory.price)}";
            levelText.GetComponent<TMP_Text>().text = $"lvl: {skillMemory.level}";
        }
    }

    public void ScaleMainPanel()
    {
        Tween.ScaleY(GetComponent<RectTransform>(),startValue:0.95f,1.05f,0.05f,ease:Ease.OutCirc);
        Tween.ScaleX(GetComponent<RectTransform>(), startValue: 1.05f, 0.95f, 0.05f, ease: Ease.OutCirc).OnComplete(() =>
        {
            Tween.ScaleY(GetComponent<RectTransform>(),1,0.05f,ease:Ease.InSine);
            Tween.ScaleX(GetComponent<RectTransform>(), 1, 0.05f, ease: Ease.InSine);
        });
    }

    public void AddPower()
    {
        switch (skillMemory.skillType)
        {
            case SkillType.Multiply:MarbleManager.instance.multiply += Mathf.FloorToInt(skillMemory.multiplier);break;
            case SkillType.Force:MarbleManager.instance.force += skillMemory.multiplier;break;
            case SkillType.CapDistance:MarbleManager.instance.distanceCap += skillMemory.multiplier;break;
        }
    }
    public void CollidedButton()
    {
        if (imageTween.isAlive)
            imageTween.Stop();
        if (rotationImageTween.isAlive)
            rotationImageTween.Stop();
        imageTween = Tween.Scale(image.GetComponent<RectTransform>(),1.05f,0.3f,Ease.OutCirc);
        rotationImageTween = Tween.Rotation(image.GetComponent<RectTransform>(),Quaternion.Euler(0,0,-4),0.3f,Ease.OutCirc);

        MouseInfo.instance.MouseInformation(skillMemory.description);

        visualPanel.GetComponent<Image>().sprite = panelWhite;
    }

    public void UncollidedButton()
    {
        if (imageTween.isAlive)
            imageTween.Stop();
        if (rotationImageTween.isAlive)
            rotationImageTween.Stop();
        imageTween = Tween.Scale(image.GetComponent<RectTransform>(),1f,0.3f,Ease.OutCirc);
        rotationImageTween = Tween.Rotation(image.GetComponent<RectTransform>(),Quaternion.Euler(0,0,0),0.3f,Ease.OutCirc);
        MouseInfo.instance.HideInfo();

        visualPanel.GetComponent<Image>().sprite = panel;

    }

}
