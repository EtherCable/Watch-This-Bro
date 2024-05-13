using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;
	public GameObject LevelCompleteTextObject;

	public AudioClip coinAudio;
	public AudioClip winAudio;
	public AudioClip respawnAudio;
	public AudioClip jumpAudio;
	public AudioClip checkpointAudio;

	private float movementX;
	private float movementY;

	public float speed = 10.0f;
	public float jumpForce = 8;
	private int score = 0;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI timerText;
	public Vector3 spawnPoint;
	private AudioSource audioSource;

	public RainbowText rainbowText;

	private float timer = 0.0f;
	private bool timerIsActive = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
		UpdateScore();
		spawnPoint = transform.position;
		LevelCompleteTextObject.SetActive(false);
		audioSource = GetComponent<AudioSource>();
    }

	void OnMove(InputValue movementValue)
	{
		Vector2 movementVector = movementValue.Get<Vector2>();

		// Get the CameraPositionController from the camera object
		CameraPositionController cameraController = Camera.main.GetComponent<CameraPositionController>();

		// If the camera is rotated, reverse the inputs
		if (cameraController.isRotated)
		{
			movementX = -movementVector.x;
			movementY = -movementVector.y;
		}
		else
		{
			movementX = movementVector.x;
			movementY = movementVector.y;
		}
	}


	void Update() {
		if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y == 0)
		{
			Vector3 jump = new Vector3(0.0f, jumpForce, 0.0f);
			rb.AddForce(jump, ForceMode.Impulse);
			audioSource.PlayOneShot(jumpAudio, 1.0f);
		}

		if (timerIsActive) // Add this line
		{
			timer += Time.deltaTime;
			int minutes = Mathf.FloorToInt(timer / 60F);
			int seconds = Mathf.FloorToInt(timer - minutes * 60);
			int milliseconds = Mathf.FloorToInt((timer - Mathf.Floor(timer)) * 1000);
			timerText.text = string.Format("Time: {0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
		}
	}

	void FixedUpdate()
	{
		Vector3 movement = new Vector3(movementX, 0.0f, movementY);
		rb.AddForce(movement * speed);

		// Max horizontal speed a player can reach
		float maxSpeed = 5f;
		Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
		if (horizontalVelocity.magnitude > maxSpeed)
		{
			horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
			rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
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
			audioSource.PlayOneShot(coinAudio, 1.0f);
		}

		if (other.gameObject.tag == "Checkpoint")
		{
			// set new spawn to cp and then remove cp cone
			spawnPoint = other.transform.position;
			other.gameObject.SetActive(false);
			audioSource.PlayOneShot(checkpointAudio, 1.0f);
		}

		if (other.gameObject.tag == "RespawnBarrier")
		{
			// if you hit respawn barrier, spawn at last cp
			transform.position = spawnPoint;
			audioSource.PlayOneShot(respawnAudio, 1.0f);
		}
	}
	
	
	void OnCollisionEnter(Collision other)
	{
		Rigidbody playerRigidbody = GetComponent<Rigidbody>(); // Assuming the player has a Rigidbody component

		if (other.gameObject.tag == "Spike")
		{
			transform.position = spawnPoint;
			audioSource.PlayOneShot(respawnAudio, 1.0f);
		}
		else if (other.gameObject.tag == "MovingPlatform")
		{
			transform.parent = other.gameObject.transform;
		}
		else if (other.gameObject.tag == "FinalPlatform")
		{
			LevelCompleteTextObject.SetActive(true);
			rainbowText.StarColorChange();
			timerIsActive = false;
			audioSource.PlayOneShot(winAudio, 1.0f);
		}
	}


	void OnCollisionStay(Collision other)
	{
		// Assuming the player has a Rigidbody component
		Rigidbody playerRigidbody = GetComponent<Rigidbody>();

		if (transform.position.y <= other.transform.position.y)
		{
			// Player is not on top of the platform, send him back to spawn
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
		scoreText.text = "Credits: " + score.ToString();
	}
	
}
