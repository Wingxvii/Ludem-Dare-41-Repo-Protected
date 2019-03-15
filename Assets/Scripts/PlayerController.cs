using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 3D MouseLook and Transform movement
/// </summary>
public class PlayerController : MonoBehaviour 
{
	[Header("Movement")]
 	[Tooltip("Player normal movement speed in Editor units")]
	public float walkingSpeed			= 2.5f;
 	[Tooltip("Player slowed movement speed, when Shift is pressed, in Editor units")]
	public float slowedWalkingSpeed		= 1.0f;

	[Header("Mouse Look")]
 	[Tooltip("Player Mouse look movement sensitivity")]
	public float mouseSensitivity		= 50f;
	[Tooltip("Reference to the viewing camera child object of Player")]
	public GameObject viewCamera;
	[Tooltip("Maximum magnitude of Y-Axis angle (Prevents looking striaght up or down)")]
	public float yAxisAngleClamp	= 75f;
	[Header("Headbob")]
	[Tooltip("Headbob magnitude")]
	public float headbobMagnitude	= 0.05f;
	[Tooltip("Headbobbing speed")]
	public float headbobSpeed 		= 4.8f;
	[Tooltip("Time smoothing headbob transition from moving to not moving.")]
	public float headbobTransitionSpeed = 20f;

	[Header("Interactions")]
 	[Tooltip("Interaction Range: Min distance to interactable in Editor Units")]
	public float interactionRange = 1f;


	[Header("Debug")]
	[Tooltip("Enable me to FLY!")]
	public bool noclip = false;

	private CharacterController _characterController;

	private float _moveSpeed;

	private float _rotX;
	private float _rotY;

	private float _initialY;

	private Vector3 _headbobRestingPosition;
	private float _headbobTimer = Mathf.PI / 2; // Sin(PI / 2) = 1;

	void Awake () 
	{
		Vector3 rot = viewCamera.transform.localRotation.eulerAngles;
		_rotX = rot.x;
		_rotY = rot.y;

		_characterController = GetComponent<CharacterController>();

		_initialY = transform.position.y;
		_headbobRestingPosition = viewCamera.transform.position;

		_moveSpeed = walkingSpeed;
	}
	
	void Update () 
	{
		MouseLookUpdate();

		if (Input.GetButton("Fire3"))
		{
			_moveSpeed = slowedWalkingSpeed;
		} else
		{
			_moveSpeed = walkingSpeed;
		}

		if (noclip)
		{
			NoClipMoveUpdate();
		} else
		{
			MoveUpdate();
		}

		InteractUpdate();

		HeadbobUpdate();
	}

	void InteractUpdate()
	{
		Debug.DrawRay(viewCamera.transform.position, viewCamera.transform.forward * interactionRange, Color.red, Time.deltaTime);

		Interactable interact = null;
		RaycastHit hit;
		if (Physics.Raycast(viewCamera.transform.position, viewCamera.transform.forward, out hit, interactionRange) && !DialogueReciever.s_instance.GetIsTyping())
		{
			interact = hit.transform.gameObject.GetComponentInChildren<Interactable>();
			if (interact)
			{
				PlayerUIController.s_instance.SetActionText(interact);
			}
		} else
		{
			PlayerUIController.s_instance.SetActionText();
		}

		if (Input.GetButtonDown("Fire1") && interact != null)
		{
			// Player attempts to interact and an interact is within range
			interact.Act();
		}
	}

	/// <summary>
	/// Simple 3D MouseLook
	/// </summary>
	void MouseLookUpdate()
	{
		float mX = Input.GetAxis("Mouse X");
		float mY = Input.GetAxis("Mouse Y");

		_rotX += -mY *	mouseSensitivity * Time.deltaTime;
		_rotY += mX	*	mouseSensitivity * Time.deltaTime;

		_rotX = Mathf.Clamp(_rotX, -yAxisAngleClamp, yAxisAngleClamp);

		Quaternion localRot = Quaternion.Euler(_rotX, _rotY, 0f);
		viewCamera.transform.rotation = localRot;
	}

	/// <summary>
	/// Simple 3D Movement
	/// </summary>
	void MoveUpdate()
	{
		Vector3 visionForward 	= viewCamera.transform.forward;
		Vector3 visionRight 	= viewCamera.transform.right;

		bool isMoving = false;

		if (Input.GetAxisRaw("Vertical") > 0)
		{
			 // Move Forward
			_characterController.Move(visionForward * _moveSpeed * Time.deltaTime);
			isMoving = true;
		} else if (Input.GetAxisRaw("Vertical") < 0)
		{
			 // Move Backward
			 _characterController.Move(-visionForward * _moveSpeed * Time.deltaTime);
			isMoving = true;
		}

		if (Input.GetAxisRaw("Horizontal") > 0)
		{
			// Strafe Right
			_characterController.Move(visionRight * _moveSpeed * Time.deltaTime);
			isMoving = true;
			
		} else if (Input.GetAxisRaw("Horizontal") < 0)
		{
			// Strafe Left
			_characterController.Move(-visionRight * _moveSpeed * Time.deltaTime);
			isMoving = true;
		}

		// Simple Z-Axis position clamping
		transform.position = new Vector3(transform.position.x,
										 _initialY,
									 	 transform.position.z);

		if (!isMoving)
		{
			_characterController.Move(Vector3.zero);
		}
	}

	void NoClipMoveUpdate()
	{
		Vector3 visionForward 	= viewCamera.transform.forward;
		Vector3 visionRight 	= viewCamera.transform.right;


		if (Input.GetAxisRaw("Vertical") > 0)
		{
			transform.Translate(visionForward * _moveSpeed); // Move Forward
		} else if (Input.GetAxisRaw("Vertical") < 0)
		{
			transform.Translate(-visionForward * _moveSpeed); // Move Backward
		}

		if (Input.GetAxisRaw("Horizontal") > 0)
		{
			transform.Translate(visionRight * _moveSpeed);// Strafe Right
		} else if (Input.GetAxisRaw("Horizontal") < 0)
		{
			transform.Translate(-visionRight * _moveSpeed);// Strafe Left
		}
	}

	void HeadbobUpdate()
	{
		Transform cameraTransform = viewCamera.GetComponentInChildren<Camera>().transform;
		if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
		{
			_headbobTimer += headbobSpeed * Time.deltaTime;
			Vector3 nextPosition = new Vector3(	cameraTransform.position.x,
												_headbobRestingPosition.y + Mathf.Abs((Mathf.Sin(_headbobTimer) * headbobMagnitude)),
												cameraTransform.position.z);
			cameraTransform.position = nextPosition;
		}
		else
		{
			_headbobTimer = Mathf.PI / 2;
			Vector3 newPosition = new Vector3(	cameraTransform.position.x,
			 									Mathf.Lerp(cameraTransform.position.y, _headbobRestingPosition.y, headbobTransitionSpeed * Time.deltaTime),
												cameraTransform.position.z);
			cameraTransform.position = newPosition;
		}
 
		if (_headbobTimer > Mathf.PI * 2)
		{
			_headbobTimer = 0;
		}
	}
}
