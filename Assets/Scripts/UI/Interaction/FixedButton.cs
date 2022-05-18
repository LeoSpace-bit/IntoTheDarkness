using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool Pressed;
    public bool DoublePressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            DoublePressed = true;
        }
        else
        {
            Pressed = true;
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;

        if (eventData.clickCount == 2)
        {
            DoublePressed = false;
        }
    }

    public void StopDoublePressed() => DoublePressed = false;

}
