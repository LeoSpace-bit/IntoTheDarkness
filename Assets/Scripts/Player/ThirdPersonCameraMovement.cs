using UnityEngine;
using Cinemachine;

public class ThirdPersonCameraMovement : MonoBehaviour
{
    [SerializeField] private FloatingJoystick _mainStick;
    [SerializeField] private CinemachineFreeLook _freeLookCam;

    [SerializeField, Range(0, 100)] private float _horizontalSensitivity = 1;
    [SerializeField, Range(0, 200)] private float _verticalSensitivity = 100;
    

    private void Update()
    {
        if(Mathf.Abs(_mainStick.Horizontal) > 0.2f)
        {
            _freeLookCam.m_XAxis.Value += _mainStick.Horizontal / _horizontalSensitivity;
        }

        if (Mathf.Abs(_mainStick.Vertical) > 0.1f)
        {
            _freeLookCam.m_YAxis.Value += (_mainStick.Vertical / _verticalSensitivity);
        }

    }
}
