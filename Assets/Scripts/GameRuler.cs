using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//TODO
// Добавить сохранение прогресса времени в сохранение
//

public class GameRuler : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Healthbar _healthbar;
    [SerializeField] private MadnessBar _madnessbar;

    [SerializeField] private Daytime _daytime;
    [SerializeField] private PortableLamp _portableLamp;

    public bool IsInCampfireArea_Burning = false;
    public bool IsDead = false;

    public UnityEvent Event_Death = new UnityEvent();

    public bool IsMadness => _isMadness;
    public bool IsNight => _isNight;

    public UnityEvent Event_ChangeDayStamp = new UnityEvent();


    private int _maxHealth = 100;
    private int _currentHealth = 100;

    private float _maxMadness = 1f;
    private float _currentMadness = 1f;

    private bool _isMadness = false;
    private bool _isNight;
    
    private void Update()
    {
        DefineMadness();
        DefineDayTime();

        if(_isMadness)
        {
            if (CurrentMadness > 0.5f)
            {
                CurrentMadness -= Time.deltaTime * 0.025f;
            }
            else if (CurrentMadness <= 0.5f && CurrentMadness > 0.1f)
            {
                CurrentMadness -= Time.deltaTime * 0.05f;
            }
            else
            {
                CurrentMadness -= Time.deltaTime * 0.01f;
            }
        }
        else
        {
            CurrentMadness = CurrentMadness >= MaxMadness ? MaxMadness : CurrentMadness + 0.025f * Time.deltaTime;

        }

    }

    private void DefineDayTime()
    {
        var nextValue = _daytime.TimeProgress >= 0.255f && _daytime.TimeProgress <= 0.78f ? false : true;

        if(_isNight != nextValue)
        {
            Event_ChangeDayStamp.Invoke();
        }

        _isNight = nextValue;
    }

    private void DefineMadness() => _isMadness = IsInCampfireArea_Burning || _portableLamp.Switch || _daytime.TimeProgress >= 0.255f && _daytime.TimeProgress <= 0.78f ? false : true;

    public float MaxMadness
    {
        get { return _maxMadness; }
        set { _maxMadness = value < 1 ? 1 : value; }
    }

    public float CurrentMadness
    {
        get { return _currentMadness; }
        set
        {
            if (value < 0)
            {
                _currentMadness = 0;
                Death();
            }
            else if (value > _maxMadness)
            {
                _currentMadness = _maxMadness;
            }
            else
            {
                _currentMadness = value;
            }

            _madnessbar.MadnessPercents = (float)(_currentMadness);
        }
    }

    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value < 1 ? 1 : value; }
    }

    public int CurrentHealth
    {
        get { return _currentHealth; }
        set 
        { 
            if(value < 1)
            {
                _currentHealth = 0;
                Death();
            }
            else if (value > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
            else
            {
                _currentHealth = value;
            }

            _healthbar.HealthPercents = (float)((float)_currentHealth / (float)_maxHealth);
        }
    }

    public void Death()
    {
        if(IsDead == false)
        {
            IsDead = true;
            Event_Death.Invoke();
        }
    }

    public void Resurrect()
    {
        CurrentHealth = MaxHealth;
        CurrentMadness = MaxMadness;
        IsDead = false;
    }
}
