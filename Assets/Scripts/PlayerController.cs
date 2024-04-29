using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;

	private float movementX;
	private float movementY;
	private float movementJump = 0;

	public float speed = 10.0f;
	public float scrambleSpeed = 5.0f;
	public float jumpSpeedHeight = 200.0f;
	private int score = 0;
	public TextMeshProUGUI scoreText;
	private Vector3 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
		UpdateScore();
		spawnPoint = transform.position;
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
		// Assuming 'platform' is your moving platform object
		if(transform.parent != null && transform.parent.gameObject.tag == "MovingPlatform")
		{
			transform.position = new Vector3(
        		transform.parent.position.x + relativePosition.x,
        		transform.position.y, // Keep the current Y position of the player
        		transform.parent.position.z + relativePosition.z
    		);
		}

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

	private Vector3 relativePosition;

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "MovingPlatform")
		{
			// Make the platform the parent of the player
			transform.parent = other.gameObject.transform;
			// Calculate the player's position relative to the platform
			relativePosition = transform.position - other.gameObject.transform.position;
		}

		if (other.gameObject.tag == "StreetCred")
		{
			// make coin disappear
			other.gameObject.SetActive(false);
			// update score and show in UI
			score++;
			UpdateScore();
		}

		if (other.gameObject.tag == "Checkpoint")
		{
			// set new spawn to cp and then remove cp cone
			spawnPoint = other.transform.position;
			other.gameObject.SetActive(false);
		}

		if (other.gameObject.tag == "RespawnBarrier")
		{
			// if you hit respawn barrier, spawn at last cp
			transform.position = spawnPoint;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "MovingPlatform")
		{
			// Remove the platform as the parent of the player
			transform.parent = null;
		}
	}

	void UpdateScore()
	{
		scoreText.text = "Creds: " + score.ToString();
	}
	
}
