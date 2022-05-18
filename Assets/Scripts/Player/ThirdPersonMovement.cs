using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Animator _animator;

    [Header("Virtual input")]
    [SerializeField] private FloatingJoystick _mainStick;
    [SerializeField] private FixedButton _jumpButton;
    [SerializeField] private FixedButton _runningButton;

    [Header("Stats")]
    [SerializeField] private float _speed = 2.5f;
    [SerializeField] private float _runSpeed = 6f;
    [SerializeField] private float _gravityForce = 9.8f;
    [SerializeField] private float _jumpForce = 19.6f;

    [Header("Settings")]
    [SerializeField, Range(0, 10)] private float _stickSensitivity = 3;
    [SerializeField] private float _turnSmoothTime = 0.1f;

    private float _turnSmoothVelocity;

    private void Update()
    {
        float horizontal = _mainStick.Horizontal * _stickSensitivity + Input.GetAxis("Horizontal");
        float vertical = _mainStick.Vertical * _stickSensitivity + Input.GetAxis("Vertical");

        float animationMagnitude = Vector2.ClampMagnitude(new Vector2(horizontal, vertical), 1).magnitude;

        float forwardVelocity;
        if (_runningButton.Pressed || Input.GetKeyDown(KeyCode.LeftShift) )
        {
            animationMagnitude = 1.5f;
            forwardVelocity = _runSpeed;
        }
        else
        {
            forwardVelocity = _speed;
        }

        // !ANIM! _animator.SetFloat("movementSpeed", animationMagnitude);

        float verticalInpulse = 1;
        if (_characterController.isGrounded)
        {
            Debug.Log("[GRAVITY] On Ground");

            verticalInpulse = -_gravityForce;
            if (_jumpButton.Pressed || Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Key is pressed: JUMP");
                verticalInpulse = _jumpForce;
            }

        }
        else
        {
            Debug.Log("[GRAVITY] Not On Ground");
            verticalInpulse = -_gravityForce;
        }
        //verticalInpulse  *= Time.deltaTime;


        Vector3 FrameVelocity = new Vector3(horizontal, verticalInpulse, vertical);

        Vector3 direction = new Vector3(horizontal, verticalInpulse, vertical).normalized;
        
        if (direction.magnitude >= 0.1)
        {
            float targetAngel = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angel = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngel, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angel, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngel, 0f) * Vector3.zero;
            moveDir += FrameVelocity;

            _characterController.Move(moveDir * forwardVelocity * Time.deltaTime);
        }


    }
}


//private void Update()
//{
//    float horizontal = _mainStick.Horizontal * _stickSensitivity + Input.GetAxis("Horizontal");
//    float vertical = _mainStick.Vertical * _stickSensitivity + Input.GetAxis("Vertical");

//    _animator.SetFloat("movementSpeed", Vector2.ClampMagnitude(new Vector2(horizontal, vertical), 1).magnitude);

//    float verticalInpulse = 0f;
//    if (!_characterController.isGrounded)
//    {
//        verticalInpulse = -(_gravityForce / _speed);

//    }
//    else
//    {
//        if (_jumpButton.Pressed)
//        {
//            //todo using anim
//        }
//    }

//    Vector3 direction = new Vector3(horizontal, verticalInpulse, vertical).normalized;

//    if (direction.magnitude >= 0.1)
//    {
//        float targetAngel = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
//        float angel = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngel, ref _turnSmoothVelocity, _turnSmoothTime);
//        transform.rotation = Quaternion.Euler(0f, angel, 0f);

//        Vector3 moveDir = Quaternion.Euler(0f, targetAngel, 0f) * Vector3.zero;
//        moveDir += direction;

//        _characterController.Move(moveDir * _speed * Time.deltaTime);
//    }

//}


//public class ThirdPersonMovement : MonoBehaviour
//{
//    [SerializeField] private CharacterController _characterController;
//    [SerializeField] private FloatingJoystick _mainStick;
//    [SerializeField] private Transform _camera;


//    [SerializeField, Range(0, 10)] private float _stickSensitivity = 3;
//    [SerializeField] private float _sensevityRotation = 7;
//    [SerializeField] private float _speed = 6f;
//    [SerializeField] private float _gravityForce = 9.8f;

//    [SerializeField] private float _turnSmoothTime = 0.1f;

//    private float _turnSmoothVelocity;

//    private void Update()
//    {
//        float horizontal = _mainStick.Horizontal * _stickSensitivity + Input.GetAxis("Horizontal");
//        float vertical = _mainStick.Vertical * _stickSensitivity + Input.GetAxis("Vertical");

//        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

//        if (!_characterController.isGrounded)
//        {
//            direction.y = -_gravityForce;
//        }

//        if (direction.magnitude >= 0.1)
//        {
//            float targetAngel = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
//            float angel = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngel, ref _turnSmoothVelocity, _turnSmoothTime);
//            transform.rotation = Quaternion.Euler(0f, angel, 0f);

//            Vector3 moveDir = Quaternion.Euler(0f, targetAngel, 0f) * Vector3.forward;

//            _characterController.Move(moveDir.normalized * _speed * Time.deltaTime);

//            Debug.Log($"targetAngel: {targetAngel} angel: {angel} transform.rotation {transform.rotation}\nmoveDir {moveDir}\n_characterController.Move({moveDir.normalized} * {_speed}\");
//        }

//    }
//}

//float horizontal = _mainStick.Horizontal * _stickSensitivity + Input.GetAxis("Horizontal");
//float vertical = _mainStick.Vertical * _stickSensitivity + Input.GetAxis("Vertical");

//Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

//if (!_characterController.isGrounded)
//{
//    direction.y = -_gravityForce;
//}

//if (direction.magnitude >= 0.1)
//{
//    float targetAngel = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
//    float angel = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngel, ref _turnSmoothVelocity, _turnSmoothTime);
//    transform.rotation = Quaternion.Euler(0f, angel, 0f);

//    Vector3 moveDir = Quaternion.Euler(0f, targetAngel, 0f) * Vector3.forward;

//    _characterController.Move(moveDir.normalized * _speed * Time.deltaTime);
//}
