using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public bool HoldStatus { get; private set; }
    public bool ClickedStatus { get; private set; }
    readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        

    public void OnPointerDown(PointerEventData eventData)
    {
        HoldStatus = true;
        ClickedStatus = true;
        StartCoroutine(StopClickEvent());
    }    

    //Wait for next update then release the click event
    IEnumerator StopClickEvent()
    {
        yield return waitForEndOfFrame;
        ClickedStatus = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HoldStatus = false;
    }
}
