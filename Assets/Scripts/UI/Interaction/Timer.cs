using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private UnityEvent Event_ResetActions;
    [SerializeField] private UnityEvent Event_ResultActions;

    [SerializeField] private Text _timer;
    [SerializeField] private float _time = 5;

    private float _currentTime;

    private void Start() => _currentTime = _time;


    public void Begin()
    {
        StartCoroutine(UpdateTimer());
    }

    public void ResetTimer()
    {
        _currentTime = _time;
        _timer.text = _currentTime.ToString();
        Event_ResetActions.Invoke();
    }

    private IEnumerator UpdateTimer()
    {
        while(_currentTime > 0)
        {
            _currentTime--;
            _timer.text = _currentTime.ToString();
            yield return new WaitForSeconds(1);
        }

        End();
    }

    private void End()
    {
        _timer.text = "";
        Event_ResultActions.Invoke();
    }

}
