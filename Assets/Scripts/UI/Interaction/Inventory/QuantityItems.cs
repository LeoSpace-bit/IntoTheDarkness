using System;
using UnityEngine;
using UnityEngine.UI;

public class QuantityItems : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Text _quantity;

    [SerializeField] private Image _ownImage;

    [SerializeField] private Color _disable;
    [SerializeField] private Color _enable;

    private int _requiredAmount;
    private int _availableQuantity;


    public int StepRequiredAmount { get; private set; }

    public int RequiredAmount
    {
        get { return _requiredAmount; }
        set
        {
            _requiredAmount = value;
            _quantity.text = $"{_availableQuantity} / {_requiredAmount}";
            ChangeColor();
        }
    }

    public int AvailableQuantity
    {
        get { return _availableQuantity; }
        set
        {
            _availableQuantity = value;
            _quantity.text = $"{_availableQuantity} / {_requiredAmount}";
            ChangeColor();
        }
    }

    public Image Icon
    {
        get { return _icon; }
        set { _icon = value; }
    }

    public QuantityItems(Image icon, int requiredAmount, int availableQuantity)
    {
        _icon = icon ?? throw new ArgumentNullException(nameof(icon));
        RequiredAmount = requiredAmount;
        AvailableQuantity = availableQuantity;

        SetStep(requiredAmount);
    }

    public void SetStep(int stepRequiredAmount)
    {
        StepRequiredAmount = stepRequiredAmount;
    }

    private void ChangeColor() => _ownImage.color = _availableQuantity >= _requiredAmount ? _enable : _disable;

}
