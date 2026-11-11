using UnityEngine;

public class NumberSpawner : MonoBehaviour
{
    public Color lowColor = Color.green;
    public Color mediumColor = Color.yellow;
    public Color highColor = Color.red;


    public static NumberSpawner instance;

    [SerializeField] GameObject number1;
    void Start()
    {
        if (instance == null)
            instance = this;
    }
    public void SpawnNumber(Transform position, System.Numerics.BigInteger value, float hitStrength)
    {
        GameObject numSp = Instantiate(number1, transform, false);
        numSp.GetComponent<NumberSp>().numberText.text = $"${value}";
        SetColor(hitStrength, numSp);
        WorldToUiUtil.SetPos(numSp.GetComponent<RectTransform>(),position);
    }

    public void SetColor(float value, GameObject numSp)
    {
        // Assume 1 = low, 2 = medium, 3 = high
        float normalized = Mathf.Clamp01((value - 1) / 2f); // 0 for 1, 0.5 for 2, 1 for 3

        Color color;

        if (normalized < 0.5f)
        {
            color = Color.Lerp(lowColor, mediumColor, normalized / 0.5f);
        }
        else
        {
            color = Color.Lerp(mediumColor, highColor, (normalized - 0.5f) / 0.5f);
        }

        float min = 0.6f;
        float max = 1.0f;

        float final = min + (max - min) * normalized; // value is 0..1

        numSp.GetComponent<NumberSp>().numberText.GetComponent<RectTransform>().localScale = Vector2.one * final;
        numSp.GetComponent<NumberSp>().numberText.color = color;
    }

}
