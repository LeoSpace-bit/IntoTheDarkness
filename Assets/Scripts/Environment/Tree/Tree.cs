using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;

public class Tree : MonoBehaviour, IDamageable
{
    [SerializeField] private int _durability = 100;
    
    [SerializeField] private GameObject _log;
    [SerializeField] private Rigidbody _logRB;


    public void Damage(int value, Vector3 from)
    {
        if(_durability - value > 0)
        {
            _durability -= value;
        }
        else
        {
            _durability = 0;
            Debug.Log("Tree crushed!");

            _log.transform.parent = null;
            _log.SetActive(true);
            _logRB.AddForce(from * 50, ForceMode.Impulse);
            gameObject.SetActive(false);
            
            

        }

    }

    
}
