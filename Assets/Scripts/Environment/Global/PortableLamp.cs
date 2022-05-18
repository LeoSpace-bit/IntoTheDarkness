using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PortableLamp : MonoBehaviour
{
    [SerializeField] private bool _switch = false;
    [SerializeField] private List<Light> lights = new List<Light>();


    [SerializeField] private Image _canvas;
    [SerializeField] private Sprite _stateOn;
    [SerializeField] private Sprite _stateOff;

    [SerializeField] private Inventory _inventory;

    public UnityEvent Event_SayToast;

    public bool Switch
    {
        get { return _switch; }
        set 
        {
            _canvas.sprite = value ? _stateOn : _stateOff;
            _switch = value;
             
        }
    }

    private bool _lampState = false;

    [SerializeField] private float _glowTime = 1;
    [SerializeField] private float _currentGlowTime = 1;



    private void Update()
    {
        if (Switch == false && _lampState == true)
        {
            lights.ForEach(light => light.enabled = false);
            _lampState = false;
        }

        if (Switch == true && _lampState == false)
        {
            lights.ForEach(light => light.enabled = true);
            _lampState = true;
        }

        if(Switch)
        {
            if (_currentGlowTime > 0)
            {
                _currentGlowTime -= Time.deltaTime;
            }
            else
            {
                if (_inventory.InventoryItems[5].Amount >= 5)
                {
                    _inventory.SubtractingAmountItem(5, out int _, 5);
                    _currentGlowTime = _glowTime;
                }
                else
                {
                    Switch = false;
                    _currentGlowTime = 0;
                    Event_SayToast.Invoke();
                }
            }
        }

    }


    public void ChangeState()
    {
        var nextState = !Switch;

        if(nextState)
        {
            if(_inventory.InventoryItems[5].Amount < 5)
            {
                nextState = false;
                Event_SayToast.Invoke();
            }
            else
            {
                _inventory.SubtractingAmountItem(5, out int _, 5);
            }
        }

        Switch = nextState;
    }


}
