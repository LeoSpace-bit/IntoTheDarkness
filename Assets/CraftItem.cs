using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftItem : MonoBehaviour
{
    [SerializeField] private Text _quantityLabel;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private QuantityItems _quantityItemsPrefab;
    [SerializeField] private Recipe _recipe;

    [SerializeField] private Transform _CQICraft_container;

    private int _quantity;
    private List<QuantityItems> _quantities = new List<QuantityItems>();

    public int Quantity
    {
        get { return _quantity; }
        set { _quantity = value < 1 ? 1 : value; _quantityLabel.text = _quantity.ToString(); }
    }

    private void Start()
    {
        _inventory ??= GameObject.FindGameObjectWithTag("GlobalScript").GetComponent<Inventory>();
        Quantity = 1;

        foreach (var item in _recipe.ConsumableItems)
        {
            var quantityItems = Instantiate(_quantityItemsPrefab, _CQICraft_container);
            var currentItem = _inventory.InventoryItems[item.Uid];

            quantityItems.Icon.sprite = currentItem.UIItem.Image.sprite;
            quantityItems.AvailableQuantity = currentItem.Amount;
            quantityItems.RequiredAmount = item.Count;

            quantityItems.SetStep(quantityItems.RequiredAmount);
            _quantities.Add(quantityItems);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _recipe.ConsumableItems.Count; i++)
        {
            var uid = _recipe.ConsumableItems[i].Uid;
            var inv = _inventory.InventoryItems[uid];
            var ia = inv.Amount;

            _quantities[i].AvailableQuantity = ia;
        }
    }


    public void IncreaseQuantity(int value = 1)
    {
        Quantity += value;

        foreach (var item in _quantities)
        {
            item.RequiredAmount += item.StepRequiredAmount;
        }

    }

    public void ReduceQuantity(int value = 1)
    {
        Quantity -= value;

        foreach (var item in _quantities)
        {
            var subvalue = item.RequiredAmount - item.StepRequiredAmount;

            if (subvalue >= item.StepRequiredAmount)
            {
                item.RequiredAmount -= item.StepRequiredAmount;
            }
        }
    }

    private int GetCountByRecipe(int uid)
    {
        foreach (var item in _recipe.ConsumableItems)
        {
            if (item.Uid == uid) return item.Count;
        }

        return -1;
    }


    public void MultiCrafting()
    {
        bool _isDoCrafting = true;
        var usedItems = new List<(int uid, int backUpAmount)>();

        foreach (var item in _recipe.ConsumableItems)
        {
            var value = _inventory.GetItem(item.Uid).Amount;

            if (_inventory.SubtractingAmountItem(item.Uid,out int availableQuantity, item.Count * Quantity))
            {
                usedItems.Add((item.Uid, value));
            }
            else
            {
                _isDoCrafting = false;
            }
        }

        if (_isDoCrafting)
        {
            _inventory.AddAmountItem(_recipe.UIDItemReceived, _recipe.UIDItemReceivedAmount * Quantity); //Debug.Log("Crafting succses");
        }
        else
        {
            foreach (var item in usedItems)
            {
                var recipeCount = GetCountByRecipe(item.uid);
                _inventory.AddAmountItem(item.uid, item.backUpAmount > Quantity * recipeCount ? Quantity *  recipeCount :  item.backUpAmount); //Debug.Log("Crafting less");
            }
        }
    }

    public void Crafting()
    {
        bool _isDoCrafting = true;
        var usedItems = new List<(int uid, int backUpAmount)>();

        foreach (var item in _recipe.ConsumableItems)
        {
            var value = _inventory.GetItem(item.Uid).Amount;

            if (_inventory.SubtractingAmountItem(item.Uid, out int availableQuantity))
            {
                usedItems.Add((item.Uid, value));
            }
            else
            {
                _isDoCrafting = false;
            }
        }

        if (_isDoCrafting)
        {
            _inventory.AddAmountItem(_recipe.UIDItemReceived, _recipe.UIDItemReceivedAmount); //Debug.Log("Crafting succses");
        }
        else
        {
            foreach (var item in usedItems)
            {
                var recipeCount = GetCountByRecipe(item.uid);
                _inventory.AddAmountItem(item.uid, item.backUpAmount > recipeCount ? recipeCount : item.backUpAmount); //Debug.Log("Crafting less");
            }
        }
    }

}
