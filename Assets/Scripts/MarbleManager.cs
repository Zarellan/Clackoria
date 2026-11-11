using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using PrimeTween;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarbleManager : MonoBehaviour
{
    #region "variables"
    public BigInteger money = 9999999;
    public BigInteger multiply = 1;
    public float force = 50f;
    public float distanceCap = 5;

    public ClampPlace clampPlace;
    #endregion
    public static MarbleManager instance;
    [SerializeField] GameObject textMarbleName;
    public bool ball_collided;
    public string marble_name;
    [SerializeField] GameObject marblePrefab;

    [SerializeField] ClackerDataScriptable marbDef;

    public int maxBalls = 20;

    public static SaveManager temporarySave; // for scene moves
    void Awake()
    {
        if (instance == null)
            instance = this;
        if (temporarySave == null)
            temporarySave = new SaveManager();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (temporarySave != null && temporarySave.marbles != null && temporarySave.marbles.Count > 0)
        {
            LoadFromTemp();
            return;
        }

        // fallback (fresh start)
        for (int i = 0; i < 2; i++)
        {
            GameObject obj = Instantiate(marblePrefab, transform);
            obj.GetComponent<Marble>().SetMarble(
                "Marble",
                ClackerData.Convert(marbDef),
                1,
                ShopSystem.colorChoices[Random.Range(0, ShopSystem.colorChoices.Length)]
            );
            obj.transform.position += new UnityEngine.Vector3(Random.Range(-0.1f, 0.1f), 0);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SaveGame();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            MoveSceneTest();
        }
    }

    public void SetMarbleName(string name)
    {
        marble_name = name;
        textMarbleName.GetComponent<TMP_Text>().text = name;
    }

    public void AddValue(BigInteger value)
    {
        money += value * multiply;
        InfoSystem.instance.TweenMoneyText();
    }

    public void SaveGame()
    {
        SaveManager save = new SaveManager();

        save.money = money.ToString();
        save.multiply = multiply.ToString();
        save.force = force;
        save.distanceCap = distanceCap;
        save.maxBalls = maxBalls;
        save.marbles = CountMarbles();
        print(ShopSystem.clackerItemsBack.Values.ToList()[0].itemName);
        save.clackerItemsBack.dict = ShopSystem.clackerItemsBack.Values.ToList();
        save.skillItemsBack.dict = ShopSystem.skillItemsBack.Values.ToList();
        DataSaver.SaveJson(save, "saveSystem",true, Application.dataPath + "/JsonData/", true);
        //DataSaver.SaveJsonEncrypted(save, "saveSystem", Application.dataPath + "/JsonData/", true);
    }
    public void LoadGame()
    {
        SaveManager load = DataSaver.LoadJson<SaveManager>("saveSystem", Application.dataPath + "/JsonData/");
        //SaveManager load = DataSaver.LoadJsonEncrypted<SaveManager>("saveSystem", Application.dataPath + "/JsonData/");

        money = BigInteger.Parse(load.money);
        multiply = BigInteger.Parse(load.multiply);
        force = load.force;
        distanceCap = load.distanceCap;
        maxBalls = load.maxBalls;
        SetMarbles(load.marbles);
        ShopSystem.clackerItemsBack = load.clackerItemsBack.dict
            .ToDictionary(x => x.itemName, x => x);
        ShopSystem.skillItemsBack = load.skillItemsBack.dict
            .ToDictionary(x => x.itemName, x => x);

    }
    List<SaveMarble> CountMarbles()
    {
        List<SaveMarble> marbles = new List<SaveMarble>();
        GameObject[] marb = GameObject.FindGameObjectsWithTag("Marble");
        for (int i = 0;i < marb.Length; i++)
        {
            Marble m = marb[i].GetComponent<Marble>();
            marbles.Add(new SaveMarble(marb[i].transform.position,new SaveMarbleData(m.marbleName, m.clackDat, m.value.ToString(),m.colorSprite))); //price is useless
        }
        return marbles;
    }

    void MoveSceneTest()
    {
        // temporarySave = 

        // temporarySave.money = money.ToString();
        // temporarySave.multiply = multiply.ToString();
        // temporarySave.force = force;
        // temporarySave.distanceCap = distanceCap;
        // temporarySave.maxBalls = maxBalls;
        // temporarySave.marbles = CountMarbles();
        // temporarySave.skillItemsBack.dict = ShopSystem.skillItemsBack.Values.ToList();

        // SceneManager.LoadScene("MarbleScene");
        
        // money = BigInteger.Parse(temporarySave.money);
        // multiply = BigInteger.Parse(temporarySave.multiply);
        // force = temporarySave.force;
        // distanceCap = temporarySave.distanceCap;
        // maxBalls = temporarySave.maxBalls;
        // SetMarbles(temporarySave.marbles);
        // ShopSystem.clackerItemsBack = temporarySave.clackerItemsBack.dict
        //     .ToDictionary(x => x.itemName, x => x);
        // ShopSystem.skillItemsBack = temporarySave.skillItemsBack.dict
        //     .ToDictionary(x => x.itemName, x => x);

        temporarySave.money = money.ToString();
        temporarySave.multiply = multiply.ToString();
        temporarySave.force = force;
        temporarySave.distanceCap = distanceCap;
        temporarySave.maxBalls = maxBalls;
        temporarySave.marbles = CountMarbles();

        temporarySave.clackerItemsBack.dict = ShopSystem.clackerItemsBack.Values.ToList();
        temporarySave.skillItemsBack.dict = ShopSystem.skillItemsBack.Values.ToList();

        SceneManager.LoadScene("MarbleScene");

    }
    void LoadFromTemp()
    {
        money = BigInteger.Parse(temporarySave.money);
        multiply = BigInteger.Parse(temporarySave.multiply);
        force = temporarySave.force;
        distanceCap = temporarySave.distanceCap;
        maxBalls = temporarySave.maxBalls;

        SetMarbles(temporarySave.marbles);

        ShopSystem.clackerItemsBack = temporarySave.clackerItemsBack.dict
            .ToDictionary(x => x.itemName, x => x);

        ShopSystem.skillItemsBack = temporarySave.skillItemsBack.dict
            .ToDictionary(x => x.itemName, x => x);
    }
    // void SetMarbles(List<SaveMarble> marbs)
    // {
    //     List<SaveMarble> marbles = marbs;
    //     for (int i = 0;i < marbles.Count; i++)
    //     {
    //         GameObject marb = Instantiate(marblePrefab,marbs[i].marblePos,UnityEngine.Quaternion.identity);
    //         marb.GetComponent<Marble>().SetMarble(marbs[i].marbleComp.marbleName, marbs[i].marbleComp.clack , BigInteger.Parse(marbs[i].marbleComp.value), marbs[i].marbleComp.col);
    //     }

    // }

    void SetMarbles(List<SaveMarble> marbs)
    {
        foreach (var m in GameObject.FindGameObjectsWithTag("Marble"))
            Destroy(m);

        for (int i = 0; i < marbs.Count; i++)
        {
            GameObject marb = Instantiate(marblePrefab, marbs[i].marblePos, UnityEngine.Quaternion.identity);
            marb.GetComponent<Marble>().SetMarble(
                marbs[i].marbleComp.marbleName,
                marbs[i].marbleComp.clack,
                BigInteger.Parse(marbs[i].marbleComp.value),
                marbs[i].marbleComp.col
            );
        }
    }

    Tween shopVisDuration;
    Tween shopVis;
    [SerializeField] float durationHudVis;
    public void LowVisShopHud()
    {
        if (shopVisDuration.isAlive)
            shopVisDuration.Stop();
        else
        {
            if (shopVis.isAlive)
                shopVis.Stop();

            shopVis =Tween.Custom(1, 0.3f, duration: 0.1f, onValueChange: newVal => {
                ShopSystem.instance.GetComponent<CanvasGroup>().alpha = newVal;
            });
        }

        shopVisDuration = Tween.Delay(durationHudVis).OnComplete(() =>
        {
            if (shopVis.isAlive)
                shopVis.Stop();
            shopVis = Tween.Custom(0.3f, 1, duration: 1, onValueChange: newVal => {
                ShopSystem.instance.GetComponent<CanvasGroup>().alpha = newVal;
            });

        });
    }


}

[System.Serializable]
public class SaveManager
{
    public string money;
    public string multiply;
    public float force;
    public float distanceCap;

    public int maxBalls;

    public List<SaveMarble> marbles;

    public DictWrapper<ClackerData> clackerItemsBack = new DictWrapper<ClackerData>();
    public DictWrapper<SkillData> skillItemsBack = new DictWrapper<SkillData>();

}
[System.Serializable]
public class SaveMarble
{
    public UnityEngine.Vector2 marblePos;
    public SaveMarbleData marbleComp;

    public SaveMarble(UnityEngine.Vector2 marblePos, SaveMarbleData marbleComp)
    {
        this.marblePos = marblePos;
        this.marbleComp = marbleComp;
    }
}

[System.Serializable]
public class SaveMarbleData
{
    public string marbleName;
    public string value;
    public ClackerData clack;
    public Color col;

    public SaveMarbleData(string marbleName, ClackerData clack, string value, Color col)
    {
        this.marbleName = marbleName;
        this.value = value;
        this.clack = clack;
        this.col = col;
    }

}
[System.Serializable]
public class DictWrapper<T>
{
    public string name;
    public List<T> dict = new List<T>();

}


[System.Serializable]
public class ClampPlace
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

}