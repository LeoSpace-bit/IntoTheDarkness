using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdsAction : MonoBehaviour
{
    [SerializeField] private UnityEvent Event_Resume;


    public void Resume()
    {
        Event_Resume.Invoke();
    }

}
