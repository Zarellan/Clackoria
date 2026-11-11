using PrimeTween;
using TMPro;
using UnityEngine;

public class NumberSp : MonoBehaviour
{
    Vector2 scale;
    public TMP_Text numberText;
    void Start()
    {
        scale = GetComponent<RectTransform>().localScale;

        GetComponent<RectTransform>().localScale = Vector2.zero;

        Tween.Scale(GetComponent<RectTransform>(), scale, 0.3f, Ease.OutBack);
        Tween.UIAnchoredPositionY(GetComponent<RectTransform>(),GetComponent<RectTransform>().anchoredPosition.y + 60,0.3f,Ease.Linear).OnComplete(()=>
        {
            Tween.Delay(1.5f).OnComplete(()=>
            {
                Tween.Alpha(gameObject.GetComponent<TMP_Text>(),0,0.5f).OnComplete(()=>
                {
                    Destroy(this.gameObject);
                });
            });
            Tween.UIAnchoredPositionY(GetComponent<RectTransform>(), GetComponent<RectTransform>().anchoredPosition.y + 100, 2f, Ease.Linear);
        });
    }
}
