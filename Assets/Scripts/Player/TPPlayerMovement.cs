using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPPlayerMovement : MonoBehaviour
{
	[Header("Relations")]
	[SerializeField] private Transform _camera;
	[SerializeField] private CharacterController _characterController;
	[SerializeField] private Animator _animator;

	[Header("Virtual inputs")]
	[SerializeField] private FloatingJoystick _mainStick;
	[SerializeField] private FixedButton _jumpButton;
	[SerializeField] private FixedButton _runningButton;

	[Header("Properties")]
	[SerializeField] private float _walkSpeed = 2;
	[SerializeField] private float _runSpeed = 6;
	[SerializeField] private float _jumpHeight = 1;
	[SerializeField] private float _gravity = -9.8f;

	[Header("Jump")]
	[SerializeField, Range(0, 1)] private float airControlPercent = 0.1f;

	[Header("SmoothTime")]
	[SerializeField] private float turnSmoothTime = 0.2f;
	[SerializeField] private float speedSmoothTime = 0.1f;

	[Header("Stats")]
	[SerializeField] private float _stamina = 10f;
	[SerializeField] private float _staminaRecoverySpeed = 1f;
	[SerializeField] private float _maxStamina = 10f;
	public float Stamina { get => _stamina; private set => _stamina = value < 1 ? 0 : value > _maxStamina ? _maxStamina : value; }
	public float MaxStamina { get => _maxStamina; private set => _maxStamina = value < 1 ? 1 : value ; }
	public float StaminaRecoverySpeed { get => _staminaRecoverySpeed; private set => _staminaRecoverySpeed = value; }

	//Inner filds
	private float turnSmoothVelocity;
	private float speedSmoothVelocity;
	private float currentSpeed;
	private float velocityY;
	private float runningEnduranceLimit = 2f;

    void Update()
	{
		#region INPUT
		//Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));	// For PC test
		Vector2 input = new Vector2(_mainStick.Horizontal, _mainStick.Vertical);						// For Touch device test

		Vector2 inputDir = input.normalized;
		bool running = _runningButton.DoublePressed | _runningButton.Pressed || Input.GetKey(KeyCode.LeftShift);
		#endregion

		if (_stamina < _maxStamina)
        {
            _stamina += _staminaRecoverySpeed * Time.deltaTime;
        }
        else if (_stamina >= _maxStamina)
        {
            _stamina = _maxStamina;
        }

		if(_stamina < runningEnduranceLimit)
        {
			running = false;
        }

		Move(inputDir, running, 0.6f, 1f);

		if (_jumpButton.Pressed || Input.GetKeyDown(KeyCode.Space))
		{
			Jump(.55f);
		}

		var magnitudeMove = Vector2.ClampMagnitude(inputDir, 1).magnitude;
		_animator.SetFloat("Velocity", magnitudeMove);
		_animator.SetBool("isRun", magnitudeMove > 0.1 ? running : false);
	}

	void Move(Vector2 inputDir, bool running, float walkingCost, float runningCost)
	{
		var deltaStamina = running ? runningCost * Time.deltaTime : walkingCost * Time.deltaTime;

		if (inputDir != Vector2.zero && _stamina - deltaStamina > 0.1)
		{
			float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + _camera.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));

			_stamina -= deltaStamina;
		}

		float targetSpeed = ((running) ? _runSpeed : _walkSpeed) * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

		velocityY += Time.deltaTime * _gravity;
		Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

		_characterController.Move(velocity * Time.deltaTime);
		currentSpeed = new Vector2(_characterController.velocity.x, _characterController.velocity.z).magnitude;

		if (_characterController.isGrounded)
		{
			velocityY = 0;
		}
	}

	void Jump(float cost)
	{
		if (_characterController.isGrounded && _stamina >= cost)
		{
			velocityY = Mathf.Sqrt(-2 * _gravity * _jumpHeight);
			_stamina -= cost;
		}
	}

	float GetModifiedSmoothTime(float smoothTime)
	{
		if (_characterController.isGrounded)
		{
			return smoothTime;
		}

		if (airControlPercent == 0)
		{
			return float.MaxValue;
		}

		return smoothTime / airControlPercent;
	}

}
