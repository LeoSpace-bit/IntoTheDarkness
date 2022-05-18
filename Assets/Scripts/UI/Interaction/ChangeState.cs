using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeState : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private bool _changeTo = true;
    [SerializeField] private GameObject _target;
    [SerializeField] private bool _disableTrigger = true;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_disableTrigger)
        {
            gameObject.SetActive(false);
        }

        _target.SetActive(_changeTo);
    }
}
