using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;

	private float movementX;
	private float movementY;
	private float movementJump = 0;

	public float speed = 10.0f;
	public float scrambleSpeed = 5.0f;
	public float jumpSpeedHeight = 200.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

	void OnMove(InputValue movementValue)
	{
		Vector2 movementVector = movementValue.Get<Vector2>();

		movementX = movementVector.x;
		movementY = movementVector.y;
	}

	void OnJump(InputValue jumpValue)
	{
		movementJump = jumpValue.Get<float>();
	}

	void FixedUpdate()
	{
		Vector3 movement = new Vector3(movementX, 0.0f, movementY);

		RaycastHit hit;

		// Apply movement force regardless of whether player is on the ground or in the air
		rb.AddForce(movement * speed);

		if (Physics.Raycast(transform.position, Vector3.down, 1.1f))
		{
			rb.AddForce(Vector3.up * movementJump * jumpSpeedHeight);
		}
		else if (Physics.SphereCast(transform.position + Vector3.down*0.25f, 0.5f, Vector3.down, out hit, 0.5f))
		{
			rb.AddForce(Vector3.up * (Physics.gravity.magnitude*0.8f + scrambleSpeed*movementJump));
		}
		movementJump = 0;
	}
}
