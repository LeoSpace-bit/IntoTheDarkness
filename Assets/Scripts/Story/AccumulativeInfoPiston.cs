using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AccumulativeInfoPiston : MonoBehaviour
{
    [SerializeField] public UnityEvent Event_Complete = new UnityEvent();

    private Inventory _inventory;


    private void Awake()
    {
        _inventory = GameObject.FindGameObjectWithTag("GlobalScript").GetComponent<Inventory>();
        _inventory.Event_AddItems.AddListener(CheckInvetory);
    }

    public void CheckInvetory()
    {
        var items = _inventory.InventoryItems;

        if(items[2].Amount >= 20 && items[3].Amount >= 5 && items[6].Amount >=10)
        {
            Event_Complete.Invoke();
        }

    }


}
