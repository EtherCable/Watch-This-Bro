using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;
	public GameObject LevelCompleteTextObject;

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
		LevelCompleteTextObject.SetActive(false);
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

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y == 0)
        {
            Vector3 jump = new Vector3(0.0f, 10, 0.0f);
            rb.AddForce(jump, ForceMode.Impulse);
        }
	}

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        // Max speed a player can reach
        float maxSpeed = 8f;
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
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
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "MovingPlatform")
		{
			transform.parent = other.gameObject.transform;
		}
		if (other.gameObject.tag == "FinalPlatform")
		{
			LevelCompleteTextObject.SetActive(true);
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
