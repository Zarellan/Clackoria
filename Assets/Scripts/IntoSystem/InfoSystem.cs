using PrimeTween;
using TMPro;
using UnityEngine;

public class InfoSystem : MonoBehaviour
{
    public static InfoSystem instance;
    [SerializeField] GameObject money;
    TMP_Text moneyText;

    Tween moneyTextTween;
    Tween moneyTextTweenColor;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moneyText = money.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = $"${FormatNumber.Format(MarbleManager.instance.money)}";
    }

    public void TweenMoneyText()
    {
        if (moneyTextTween.isAlive)
            moneyTextTween.Stop();
    
        moneyTextTween = Tween.Scale(money.GetComponent<RectTransform>(),startValue:1.1f,endValue:1f,0.3f,Ease.OutCirc);
        moneyTextTweenColor = Tween.Color(money.GetComponent<TMP_Text>(),startValue:Color.green,endValue:Color.white,0.3f,Ease.OutCirc);

    }

}
