using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRepair : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    private bool _isConditionsMet;
    public void SetConditionsMet(bool value) => _isConditionsMet = value;


    private void OnTriggerStay(Collider other)
    {
        if(_isConditionsMet && other.tag == "Player")
        {
            _object.SetActive(true);
        }
    }
}
