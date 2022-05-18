using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonTouchCameraMovement : MonoBehaviour
{
    [SerializeField] private FixedTouchField _fixedTouchField;
    [SerializeField] private CinemachineFreeLook _freeLookCam;

    [SerializeField, Range(1, 100)] private float _horizontalSensitivity = 5;
    [SerializeField, Range(1, 500)] private float _verticalSensitivity = 250;

    private void Update()
    {
        if (_fixedTouchField.TouchDist != Vector2.zero)
        {
            _freeLookCam.m_XAxis.Value += (_fixedTouchField.TouchDist.x / _horizontalSensitivity);
            _freeLookCam.m_YAxis.Value += (_fixedTouchField.TouchDist.y / _verticalSensitivity) * -.53f;
        }
    }
}
