using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseInfo : MonoBehaviour
{
    [SerializeField] RectTransform parent;
    Camera mainCam;
    public static MouseInfo instance;
    [SerializeField] GameObject infoImage, infoText;
    [SerializeField] float extra_width;
    [SerializeField] float extra_height;
    [SerializeField] float maxWidth = 250f;


    Vector2 infoPos;
    [SerializeField] Vector3 mouseOffset = new Vector2(15, -15);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // MouseInformation("dskds d\ndfd\ngfgf\ngfgf\ngdf");
        HideInfo();
        mainCam = Camera.main;
        if (instance == null)
            instance = this;
    }

    int offsetMod = 1;
    // Update is called once per frame
    void Update()
    {
        
        if (IsTouchOutRightUI(infoImage, mouseOffset.x))
        {
            offsetMod = -1;
            infoImage.GetComponent<RectTransform>().pivot = new Vector2(1,infoImage.GetComponent<RectTransform>().pivot.y);
        }
        else
        {
            offsetMod = 1;
            infoImage.GetComponent<RectTransform>().pivot = new Vector2(0,infoImage.GetComponent<RectTransform>().pivot.y);
        }
        float scale = parent.GetComponent<Canvas>().scaleFactor;

        Vector2 offset = new Vector2(mouseOffset.x * offsetMod, mouseOffset.y) / scale;

        infoImage.GetComponent<RectTransform>().anchoredPosition = MousePositionCalc() + offset;
    }

    Vector2 MousePositionCalc()
    {
        Vector2 anchPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Input.mousePosition, mainCam, out anchPos);
        return anchPos;
    }

    bool IsTouchOutRightUI(GameObject obj, float offset)
    {
        return MousePositionCalc().x + offset + obj.GetComponent<RectTransform>().sizeDelta.x >= (parent.rect.width / 2f);
    }

    public void MouseInformation(string info)
    {
        TMP_Text tex = infoText.GetComponent<TMP_Text>();
        tex.text = info;
        tex.ForceMeshUpdate();
        Vector2 size = tex.GetPreferredValues(info, maxWidth, Mathf.Infinity);
        tex.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x, size.y);
        infoImage.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x + extra_width,size.y + extra_height);
        infoImage.GetComponent<Image>().color = new Color(1,1,1,1);
        infoPos = infoImage.GetComponent<RectTransform>().sizeDelta;
    }
    public void HideInfo()
    {
        TMP_Text tex = infoText.GetComponent<TMP_Text>();
        tex.text = "";
        infoImage.GetComponent<Image>().color = new Color(1,1,1,0);

    }

    Vector2 GetCorrectedMousePos(RectTransform canvasRect)
    {
        Vector2 screenMouse = Input.mousePosition;

        // Canvas size in pixels (RectTransform)
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        // Actual canvas size in browser (Screen.width / Screen.height)
        float scaleX = Screen.width / canvasWidth;
        float scaleY = Screen.height / canvasHeight;

        // Scale mouse position to canvas local space
        Vector2 scaledMouse = new Vector2(screenMouse.x / scaleX, screenMouse.y / scaleY);
        return scaledMouse;
    }
}
