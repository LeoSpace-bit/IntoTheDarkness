using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Daytime : MonoBehaviour
{
    [SerializeField] private Gradient _directionalLightGradient;
    [SerializeField] private Gradient _ambientLightGradient;
    [SerializeField] private Gradient _fogColorGradient;

    [SerializeField, Range(1, 3600)] private float _timeDayInSeconds = 60;
    [SerializeField, Range(0.0f, 1.0f)] private float _timeProgress;

    [SerializeField] private Light _directionLight;

    private Vector3 _defaultAngles;

    public float TimeProgress
    {
        get => _timeProgress;
        set => _timeProgress = value < 0 ? 0 : value > 1 ? 1 : value;
    }

    void Start() => _defaultAngles = _directionLight.transform.localEulerAngles;

    void Update()
    {
        if(Application.isPlaying) _timeProgress += Time.deltaTime / _timeDayInSeconds;

        if (_timeProgress > 1f) _timeProgress = 0f;

        _directionLight.color = _directionalLightGradient.Evaluate(_timeProgress);
        RenderSettings.ambientLight = _ambientLightGradient.Evaluate(_timeProgress);

        RenderSettings.fogColor = _fogColorGradient.Evaluate(_timeProgress);

        _directionLight.transform.localEulerAngles = new Vector3(360f * _timeProgress - 90, _defaultAngles.x, _defaultAngles.z);
    }
}
