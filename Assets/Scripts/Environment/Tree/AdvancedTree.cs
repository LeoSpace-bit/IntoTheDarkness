using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedTree : MonoBehaviour, IDamageable
{
    [SerializeField] private int _durability = 100;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private GameObject[] _logs;

    private bool _isFelled = false;

    public void Damage(int value, Vector3 from)
    {
        if (!_isFelled)
        {
            if (_durability - value > 0)
            {
                _durability -= value;
            }
            else
            {
                _isFelled = true;
                _durability = 100;
                Debug.Log("Tree crushed!");

                _rigidbody.constraints = RigidbodyConstraints.None;
                _rigidbody.AddForce(-from * 50, ForceMode.Impulse);
            }
        }
        else
        {
            if (_durability - value > 0)
            {
                _durability -= value;
            }
            else
            {
                Debug.Log("Loging sucsess!");

                foreach (var log in _logs)
                {
                    log.transform.parent = null;
                    log.tag = "Log";
                    log.SetActive(true);
                }

                Destroy(gameObject);
            }
        }
    }
}
