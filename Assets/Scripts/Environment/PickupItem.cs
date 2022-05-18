using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Transform _parentUI;
    [SerializeField] private GameObject _prefabUI;


    [SerializeField] private int _targetUID;
    
    private FixedButton _button;
    private GameObject _instaledPrefab;


    internal void SetTargetUID(int uid) => _targetUID = uid;

    public void Pickup()
    {
        _inventory.AddAmountItem(_targetUID);
    }

    private void Start()
    {
        _inventory ??= GameObject.FindGameObjectWithTag("GlobalScript").GetComponent<Inventory>();

        if (_parentUI == null)
        {
            _parentUI = GameObject.FindGameObjectWithTag("TIS").transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(_instaledPrefab == null)
            {
                _instaledPrefab = Instantiate(_prefabUI, _parentUI);

                _button = _instaledPrefab.GetComponent<FixedButton>();
                var banner = _instaledPrefab.GetComponent<Banner>();
                banner.Title = _inventory.GetName(_targetUID);
                banner.enabled = true;
            }
            else
            {
                _instaledPrefab.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(_button != null && _button.Pressed)
        {
            if(_inventory.InventoryItems[_targetUID].Amount < _inventory.InventoryItems[_targetUID].MaximumAmount)
            {
                _inventory.AddAmountItem(_targetUID);
                gameObject.SetActive(false);
                Destroy(_instaledPrefab);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if(_instaledPrefab != null)
            {
                _instaledPrefab.SetActive(false);
            }
        }
    }

}
