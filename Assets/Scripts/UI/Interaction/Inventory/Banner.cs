using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Banner : MonoBehaviour
{
    public string Title { get; set; }

    [SerializeField] private Text _text;

    private void Start()
    {
        _text.text = Title;
    }

}
