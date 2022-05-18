using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDemonstrator : MonoBehaviour
{
    [SerializeField] private Inventory _inventory; 
    [SerializeField] private GameObject _itemPrefab;

    private void Start()
    {
        foreach (var item in _inventory.InventoryItems)
        {
            CreateUIItem();
        }
    }

    private void CreateUIItem()
    {
        GameObject newItem = Instantiate(_itemPrefab, gameObject.transform);
        

    }

}
