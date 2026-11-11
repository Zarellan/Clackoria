using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonScript : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{



    public bool collideable = true;

    [SerializeField] UnityEvent entered, exited;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Button>() != null)
            collideable = GetComponent<Button>().interactable;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CollidableBool(bool value)
    {
        collideable = value;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (collideable)
            entered.Invoke();

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (collideable)
            exited.Invoke();
    }


    Color RGBValue(float r,float g,float b)
    {
        return new Color(r/255,g/255,b/255);
    }


}
