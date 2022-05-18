using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class ShakeCamera : MonoBehaviour
{
    [SerializeField] private MadnessBar _bar;
    [SerializeField] private CinemachineFreeLook _camera;
    private List<CinemachineBasicMultiChannelPerlin> _camerasNoise;

    [SerializeField] private NoiseSettings _noiseFear;
    [SerializeField] private NoiseSettings _noiseMadness;


    private float _amplitude
    {
        get => _bar.MadnessPercents; set
        {
            //if(value > 0.3f)
            //{
            //    _camerasNoise.ForEach((perlin) => { if (perlin.m_NoiseProfile != _noiseFear) { perlin.m_NoiseProfile = _noiseMadness; } });
            //}
            //else
            //{
            //    _camerasNoise.ForEach((perlin) => { if (perlin.m_NoiseProfile != _noiseFear) { perlin.m_NoiseProfile = _noiseFear; } } );
            //}


            _camerasNoise.ForEach((perlin) => perlin.m_AmplitudeGain = value);
        }
    }


    private void Awake()
    {
        _camera ??= GetComponent<CinemachineFreeLook>();
        
        _camerasNoise = new List<CinemachineBasicMultiChannelPerlin>
        {
            _camera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(),
            _camera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(),
            _camera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>()
        };
    }

    private void Update()
    {
        _amplitude = Mathf.Abs(_bar.MadnessPercents - 1);

        if (_bar.MadnessPercents < 0.3f)
        {
            _camerasNoise.ForEach((perlin) => { if (perlin.m_NoiseProfile != _noiseFear) { perlin.m_NoiseProfile = _noiseMadness; } });
        }
        else
        {
            _camerasNoise.ForEach((perlin) => { if (perlin.m_NoiseProfile != _noiseFear) { perlin.m_NoiseProfile = _noiseFear; } });
        }


    }


}
