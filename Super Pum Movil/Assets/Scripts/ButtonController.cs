using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool HoldStatus { get; private set; }
    public bool ClickedStatus { get; private set; }

    //Wait for next update then release the click event
    readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    
    public void OnPointerDown(PointerEventData eventData)
    {
        HoldStatus = true;
        ClickedStatus = true;
        StartCoroutine(StopClickEvent());
    }    

    
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
