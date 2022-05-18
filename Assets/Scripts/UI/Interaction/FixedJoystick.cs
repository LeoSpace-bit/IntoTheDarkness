using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FixedJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _hundle;
    [Range(0f, 2f), SerializeField] private float _handelLimit = 1f;
    private Vector2 _input = Vector2.zero;


    public void OnPointerDown(PointerEventData data)
    {
        OnDrag(data);
    }

    public void OnDrag(PointerEventData data)
    {
        Vector2 JoyDriection = data.position - RectTransformUtility.WorldToScreenPoint(new Camera(), _background.position);

        _input = (JoyDriection.magnitude > _background.sizeDelta.x / 2f) ? JoyDriection.normalized : JoyDriection / (_background.sizeDelta.x / 2f);
        _hundle.anchoredPosition = (_input * _background.sizeDelta.x / 2f) * _handelLimit;
    }

    public void OnPointerUp(PointerEventData data)
    {
        _input = Vector2.zero;
        _hundle.anchoredPosition = Vector2.zero;
    }

}
