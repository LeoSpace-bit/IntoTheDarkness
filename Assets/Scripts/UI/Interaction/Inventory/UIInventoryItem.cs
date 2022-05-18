using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
    public int UID;
    public Text Amount => _amount;
    public Image Image => _image;

    [SerializeField] private Text _amount;
    [SerializeField] private Image _image;

    [SerializeField] private Color _workingColor;
    [SerializeField] private Color _notWorkingColor;

    [SerializeField] private Image _mainImage;
    
    public UIInventoryItem(int uID, Text amount, Image image)
    {
        UID = uID;
        _amount = amount;
        _image = image;
    }

    public void SetAmount(int amount)
    {
        _mainImage.color = amount == 0 ? _notWorkingColor : _workingColor;
        _amount.text = amount.ToString();
    }
    public void SetImage(Sprite image)
    {
        _image.sprite = image;
    }

}
