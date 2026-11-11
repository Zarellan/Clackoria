using System.Collections.Generic;
using PrimeTween;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClackerItem : MonoBehaviour
{
    [SerializeField] GameObject image;
    [SerializeField] GameObject titleText;
    [SerializeField] GameObject priceText;
    [SerializeField] GameObject marblePrefab;
    [SerializeField] GameObject visualPanel;
    [SerializeField] GameObject visualPanelHelp;
    [SerializeField] Sprite panel,panelWhite;

    // public string itemName;
    // public int price;
    // public int multiplier;
    // public string description;
    // public Color color;
    // public bool isRandomColor;

    Tween imageTween, rotationImageTween;

    AudioSource audi;

    // public List<ClackerData> nextClacker;
    // public bool itemBought;
    ClackerData clackerMemory;
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
    // string name, int value, int multiply, string desc, bool isRand, Color col
    public void SetItem(ClackerData clackData)
    {
        // itemName = clackData.itemName;
        // price = clackData.price;
        // multiplier = clackData.multiplier;
        // description = clackData.description;
        clackerMemory = clackData;
        image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/ClackerItem/"+clackData.itemName);
        titleText.GetComponent<TMP_Text>().text = clackData.itemName;
        priceText.GetComponent<TMP_Text>().text = $"$ {clackData.price}";
        // color = clackData.color;
        // isRandomColor = clackData.isRandomColor;
        // nextClacker = clackData.nextClacker;
        // itemBought = clackData.itemBought;
    }

    public void BuyItem()
    {
        if (MarbleManager.instance.money >= clackerMemory.price && MarbleManager.instance.maxBalls > GameObject.FindGameObjectsWithTag("Marble").Length)
        {
            GameObject obj = Instantiate(marblePrefab);

            obj.GetComponent<Marble>().SetMarble(clackerMemory.itemName, clackerMemory ,clackerMemory.multiplier,
                    clackerMemory.isRandomColor?ShopSystem.colorChoices[Random.Range(0,ShopSystem.colorChoices.Length)]:clackerMemory.color);
            obj.GetComponent<Marble>().TweenMarble();
            // GlobalAudio.instance.PlayAudioPitch("money purchase", Random.Range(0.95f,1.1f), 1);
            audi.pitch = Random.Range(0.97f,1.03f);
            audi.Play();
            ScaleMainPanel();
            Tween.Color(visualPanelHelp.GetComponent<Image>(),new Color(1,1,1,255.0f/144.0f),new Color(1,1,1,0),0.3f,Ease.OutCirc);
            if (clackerMemory.nextClacker != default && !clackerMemory.itemBought)
            {
                for (int i = 0;i < clackerMemory.nextClacker.Count;i++)
                {
                    ShopSystem.instance.AddClackerNow(clackerMemory.nextClacker[i]);
                    
                }
                clackerMemory.itemBought = true;
            }
            MarbleManager.instance.money -= clackerMemory.price;

            MarbleManager.instance.LowVisShopHud();
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
    public void CollidedButton()
    {
        if (imageTween.isAlive)
            imageTween.Stop();
        if (rotationImageTween.isAlive)
            rotationImageTween.Stop();
        imageTween = Tween.Scale(image.GetComponent<RectTransform>(),1.05f,0.3f,Ease.OutCirc);
        rotationImageTween = Tween.Rotation(image.GetComponent<RectTransform>(),Quaternion.Euler(0,0,-4),0.3f,Ease.OutCirc);

        MouseInfo.instance.MouseInformation(clackerMemory.description);

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
