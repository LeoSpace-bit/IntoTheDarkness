using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _hundle;
    [Range(0f, 2f), SerializeField] private float _handelLimit = 1f;
    private Vector2 _input = Vector2.zero;
    private Vector2 _joystickPosition = Vector2.zero;

    public float Horizontal => _input.x;
    public float Vertical => _input.y;

    public void OnPointerDown(PointerEventData data)
    {
        _background.gameObject.SetActive(true);
        OnDrag(data);
        _joystickPosition = data.position;
        _background.position = data.position;
        _hundle.anchoredPosition = Vector2.zero;
    }

    public void OnDrag(PointerEventData data)
    {
        Vector2 JoyDriection = data.position - _joystickPosition;

        _input = (JoyDriection.magnitude > _background.sizeDelta.x / 2f) ? JoyDriection.normalized : JoyDriection / (_background.sizeDelta.x / 2f);
        _hundle.anchoredPosition = (_input * _background.sizeDelta.x / 2f) * _handelLimit;
    }

    public void OnPointerUp(PointerEventData data)
    {
        _background.gameObject.SetActive(false);
        _input = Vector2.zero;
        _hundle.anchoredPosition = Vector2.zero;
    }
}
