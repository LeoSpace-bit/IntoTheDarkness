using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExternalDataPiston : MonoBehaviour
{
    [SerializeField] public UnityEvent Event_Complete;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Event_Complete.Invoke();
        }
    }

    public void DeleteObject(GameObject gameObject) => Destroy(gameObject);

}
