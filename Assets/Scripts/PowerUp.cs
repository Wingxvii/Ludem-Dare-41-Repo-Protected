using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour 
{
	public bool isTalking = false;
	public bool isPickedUpByCars = false;

	public float rotateSpeed = 2.0f;

	public float bobbingSpeed;
	public float bobbingMagnitude;

	public GameObject player;


	private Vector3 _bobbingRestingPosition;
	private float _bobbingTimer = 0; // Sin(PI / 2) = 1;

	void Awake()
	{
		_bobbingRestingPosition = transform.position;
	}

	void Update()
	{
		if (!isTalking)
		{
			transform.Rotate(new Vector3(0, -rotateSpeed, 0));
		} else
		{
			transform.LookAt( player.transform.position );
			transform.Rotate(new Vector3(0, 180, 0));
		}

		_bobbingTimer += bobbingSpeed * Time.deltaTime;
		Vector3 nextPosition = new Vector3(	transform.position.x,
											transform.position.y + ((Mathf.Sin(_bobbingTimer) * bobbingMagnitude)),
											transform.position.z);
		transform.position = nextPosition;

		if (_bobbingTimer > Mathf.PI * 2)
		{
			_bobbingTimer = 0;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		// Hack-y check for vehicle
		if (other.gameObject.GetComponentInChildren<MoveForward>() && isPickedUpByCars)
		{
			Destroy(this.gameObject);
		}
	}
}
