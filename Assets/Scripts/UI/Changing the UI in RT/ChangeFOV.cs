using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFOV : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _freeLookCam;

    [Header("Dependencies")]
    [SerializeField] private Animator _animator;

    [Header("Setting")]
    [SerializeField, Range(0, 10)] private float _decreaseRate = 1f;
    [SerializeField, Range(0, 10)] private float _recoveryRate = 5f;

    [SerializeField] private float _FOVRunning = 30f;

    private float _initialFOV;

    private void Awake() => _initialFOV = _freeLookCam.m_Lens.FieldOfView;

    void Update()
    {
        _freeLookCam.m_Lens.FieldOfView = _animator.GetBool("isRun") ? 
            Mathf.Lerp(_freeLookCam.m_Lens.FieldOfView, _FOVRunning, Time.deltaTime * _decreaseRate) : Mathf.Lerp(_freeLookCam.m_Lens.FieldOfView, _initialFOV, Time.deltaTime * _recoveryRate);

    }
}
