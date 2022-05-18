using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _inventoryParent;

    public List<InventoryItem> InventoryItems = new List<InventoryItem>();
    public UnityEvent Event_AddItems = new UnityEvent();

    private int _lastUID = 0;

    private void Start()
    {
        /* 0 */ RegisterItem("Stone", 20, "Sprites/Icons/ICON_Stone");
        /* 1 */ RegisterItem("Log", 10, "Sprites/Icons/ICON_Log");
        /* 2 */ RegisterItem("Plank", 40, "Sprites/Icons/ICON_Plank");
        /* 3 */ RegisterItem("Protective covering", 10, "Sprites/Icons/ICON_WoodenBucket");
        /* 4 */ RegisterItem("Mushroom", 20, "Sprites/Icons/ICON_Mushroom");
        /* 5 */ RegisterItem("Wooden dust", 200, "Sprites/Icons/ICON_WoodenDust");          //фонарь питается этим
        /* 6 */ RegisterItem("Sparkling mix", 50, "Sprites/Icons/ICON_SparklingMix");       //Sparkling mix     50      1 = 2 Stone + 5 Wood dust
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Keypad4))
        {
            AddAmountItem(0);
        }

        if (Input.GetKeyUp(KeyCode.Keypad5))
        {
            AddAmountItem(1);
        }

        if (Input.GetKeyUp(KeyCode.Keypad1))
        {
            AddAmountItem(2, 10);
            AddAmountItem(3, 5);
            AddAmountItem(6, 5);
        }


    }

    private void RegisterItem(string name, int maximumAmount, string iconName, string description = "")
    {
        var newItem = Instantiate(_itemPrefab, _inventoryParent.transform);

        if(newItem.TryGetComponent<UIInventoryItem>(out UIInventoryItem inventoryItem))
        {
            inventoryItem.UID = _lastUID;
            inventoryItem.SetAmount(0);

            inventoryItem.SetImage(Resources.Load<Sprite>(iconName));
        }

        InventoryItems.Add(new InventoryItem() { UID = _lastUID, Name = name, MaximumAmount = maximumAmount, Amount = 0, Description = description, UIItem = inventoryItem });

        _lastUID++;
    }

    public void AddAmountItem(int uid, int amount = 1)
    {
        foreach (var item in InventoryItems)
        {
            if (item.UID == uid && item.Amount < item.MaximumAmount)
            {
                item.Amount += amount;
                item.UIItem.SetAmount(item.Amount);
            }
        }

        if (Event_AddItems != null) Event_AddItems.Invoke();

    }

    public bool SubtractingAmountItem(int uid, out int availableQuantity, int amount = 1)
    {
        foreach (var item in InventoryItems)
        {
            if (item.UID == uid)
            {
                availableQuantity = item.Amount - amount;
                Debug.LogWarning($"Value^ {availableQuantity}");

                if (availableQuantity >= 0)
                {
                    item.Amount -= amount;
                    item.UIItem.SetAmount(item.Amount);
                }
                else
                {
                    return false;
                }
            }
        }

        availableQuantity = -1;
        return true;
    }

    public string GetName(int uid)
    {
        foreach (var item in InventoryItems)
        {
            if (item.UID == uid)
            {
               return item.Name;
            }
        }

        return string.Empty;
    }

    public InventoryItem GetItem(int uid)
    {
        foreach (var item in InventoryItems)
        {
            if (item.UID == uid) return item;
        }

        return null;
    }


}


//public class Inventory : MonoBehaviour
//{
//    [SerializeField] private GameObject _itemPrefab;
//    [SerializeField] private GameObject _inventoryParent;

//    public List<InventoryItem> InventoryItems = new List<InventoryItem>();

//    private int _lastUID = 0;

//    private void Start()
//    {
//        RegisterItem("Stone", 20, "Sprites/Icons/ICON_Stone");
//    }


//    private void Update()
//    {
//        if (Input.GetKeyUp(KeyCode.Keypad4))
//        {
//            AddAmountItem(0);
//        }
//    }

//    private void RegisterItem(string name, int maximumAmount, string iconName, string description = "")
//    {
//        InventoryItems.Add(new InventoryItem() { UID = _lastUID, Name = name, MaximumAmount = maximumAmount, Amount = 0, Description = description });
//        var newItem = Instantiate(_itemPrefab, _inventoryParent.transform);

//        if (newItem.TryGetComponent<UIInventoryItem>(out UIInventoryItem inventoryItem))
//        {
//            inventoryItem.UID = _lastUID;
//            inventoryItem.SetAmount(0);

//            inventoryItem.SetImage(Resources.Load<Sprite>(iconName));
//        }

//        _lastUID++;
//    }

//    public void AddAmountItem(string name, int amount = 1)
//    {
//        foreach (var item in InventoryItems)
//        {
//            if (item.Name == name)
//            {
//                item.Amount += amount;
//            }
//        }
//    }

//    public void AddAmountItem(int uid, int amount = 1)
//    {
//        foreach (var item in InventoryItems)
//        {
//            if (item.UID == uid)
//            {
//                item.Amount += amount;

//                Debug.Log($"[UID {item.UID}] {item.Name}: Amount = {item.Amount}");

//            }
//        }
//    }

//}
