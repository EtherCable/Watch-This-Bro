using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Video;
using static Unity.VisualScripting.Member;

enum _state
{
    GROUNDED,
	JUMPING,
	DOUBLE_JUMP
};

public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;
	public GameObject LevelCompleteTextObject;

	public AudioClip coinAudio;
	public AudioClip winAudio;
	public AudioClip respawnAudio;
	public AudioClip jumpAudio;
	public AudioClip checkpointAudio;
	public AudioClip levelUpAudio;

	public AudioClip powerup_jump_audio;
	public AudioClip powerup_lives_audio;
	public AudioClip powerup_timeslow_audio;

	private float movementX;
	private float movementY;

	public float speed = 10.0f;
	public float jumpForce = 8;
	private int score = 0;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI timerText;
	public TextMeshProUGUI livesText;
	public Vector3 spawnPoint;
	private AudioSource audioSource;

	public RainbowText rainbowText;

	public float timer = 0.0f;
	private bool timerIsActive = true;

	public int lives = 5;
	private _state state;
	public float doubleJumpForce = 4.0f;
	public float doubleJumpForce_baseline = 4.0f;



	public int powerup_current = 0;
	public float powerup_time = 0.0f;
	public float powerrup_max_time = 0.0f;
	private float powerup_doublejump_boost = 2.5f;
    private float powerup_time_slow = 0.5f;
	public bool powerup_applied = false;

	public GameObject death_screen;
	public LevelSystem level_sys;

	private bool onPlatform;

    // Start is called before the first frame update
    void Start()
    {
		onPlatform = false;
		Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
		UpdateScore();
		UpdateLives();
		spawnPoint = transform.position;
		LevelCompleteTextObject.SetActive(false);
		audioSource = GetComponent<AudioSource>();
		state = _state.GROUNDED;
    }

	void OnMove(InputValue movementValue)
	{
		Vector2 movementVector = movementValue.Get<Vector2>();

		// Get the CameraPositionController from the camera object
		CameraPositionController cameraController = GameObject.Find("CamPivot").GetComponent<CameraPositionController>();

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
		switch (state)
		{
			case _state.GROUNDED:
                rb.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
                if (onPlatform && Input.GetKeyDown(KeyCode.Space))
                {
                    Vector3 jump = new Vector3(0.0f, jumpForce, 0.0f);
                    rb.AddForce(jump, ForceMode.Impulse);
                    audioSource.PlayOneShot(jumpAudio, 0.4f);
                    state = _state.JUMPING;
                }
                break;

			case _state.JUMPING:
			    if (Input.GetKeyDown(KeyCode.Space))
				{
					Vector3 dbjump = new Vector3(rb.velocity.x, doubleJumpForce, rb.velocity.z);
					rb.velocity = dbjump;
					audioSource.PlayOneShot(jumpAudio, 0.4f);
					state = _state.DOUBLE_JUMP;
				}
				break;

			case _state.DOUBLE_JUMP:
				// dont do anything here, just wait to land
				break;
		}

		if (timerIsActive) // Add this line
		{
			timer += Time.deltaTime;
			int minutes = Mathf.FloorToInt(timer / 60F);
			int seconds = Mathf.FloorToInt(timer - minutes * 60);
			int milliseconds = Mathf.FloorToInt((timer - Mathf.Floor(timer)) * 1000);
			timerText.text = string.Format("Time: {0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
		}

		//if (lives == 0)
		//{
		//	Die();
		//}

		if(powerup_current!=0)
		{
			//float time_mult = 1.0f;
			if(!powerup_applied)
			{
				switch (powerup_current)
				{

					case 1:
						this.doubleJumpForce = this.doubleJumpForce_baseline * this.powerup_doublejump_boost;
						audioSource.PlayOneShot(this.powerup_jump_audio);
						break;
					case 2:
						Time.timeScale = this.powerup_time_slow;
                        //time_mult = (1+ (1-this.powerup_time_slow));
                        audioSource.PlayOneShot(this.powerup_timeslow_audio);
                        break;
					case 3:
                        UpdateLives();
                        audioSource.PlayOneShot(this.powerup_lives_audio);
                        break;




                }
                powerup_applied = true;
			}
			

			if (powerup_time >= powerrup_max_time)
            {
				switch(powerup_current)
				{
					case 1:
						this.doubleJumpForce = this.doubleJumpForce_baseline;
						
						break;
					case 2:
                        Time.timeScale = 1f;

                        break;
					case 3:
						break;
				}
				Debug.Log("reset");
				powerup_current = 0;
                UpdateLives();


            }
            //powerup_time += Time.deltaTime* time_mult;
            powerup_time += Time.unscaledDeltaTime;
        }


	}
	public void PowerUp(int type, float duration)
	{
		if (powerup_current != 0) return;
        this.powerrup_max_time = duration;
        this.powerup_time = 0.0f;
        this.powerup_current = type;
		this.powerup_applied = false;

        Debug.Log("applied powerup: " + type);
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


	private void Respawn()
	{
       
        if(powerup_current != 3)
			lives--;

        UpdateLives();
        if (lives == 0)
		{
			Die();
		}
		else
		{
            transform.position = spawnPoint;
            audioSource.PlayOneShot(respawnAudio, 1.0f);
			this.onPlatform = true;
        }

        
    }

	//private Vector3 relativePosition;

	void OnTriggerEnter(Collider other)
	{
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
		else if (other.gameObject.tag == "Star")
		{
			audioSource.PlayOneShot(levelUpAudio, 1.0f);
		}

		if (other.gameObject.tag == "RespawnBarrier")
		{
			// if you hit respawn barrier, spawn at last cp
			Respawn();
		}
		if (other.gameObject.tag == "MovingPlatform" || other.gameObject.tag == "Platform" || other.gameObject.tag == "FinalPlatform")
		{
			onPlatform = true;
			state = _state.GROUNDED;
			GetComponent<Rigidbody>().AddForce(Vector3.down * 10f, ForceMode.VelocityChange);
		}

	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "MovingPlatform")
		{
			onPlatform = true;
			state = _state.GROUNDED;
		}
		if (other.gameObject.tag == "Platform")
		{
			onPlatform = true;
			state = _state.GROUNDED;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		Rigidbody playerRigidbody = GetComponent<Rigidbody>(); // Assuming the player has a Rigidbody component
		//bool plat = false;
		if (other.gameObject.tag == "Spike")
		{
			// on contact with spike, remove life, send back to last
			// spawn point, and play sound.
			Respawn();
		}
		else if (other.gameObject.tag == "MovingPlatform")
		{
			onPlatform = true;
            state = _state.GROUNDED;
            Debug.Log("grounded moving platform collision: " + other.gameObject.name + $"({Time.frameCount})");

        }
        else if (other.gameObject.tag == "Platform")
		{	
			onPlatform = true;
			state = _state.GROUNDED;
			Debug.Log("grounded  platform collision :"  + other.gameObject.name + $"({Time.frameCount})");

        }
        else if (other.gameObject.tag == "FinalPlatform")
		{
			// upon touching final platform, show complete lv UI
			// turn off timer, and play win audio
			GameObject level0 = GameObject.Find("Level0");
			GameObject level1 = GameObject.Find("Level1");
			GameObject level2 = GameObject.Find("Level2");
			if (level0 != null) {
				LevelCompleteTextObject.GetComponent<TextMeshProUGUI>().text = "Tutorial Level\nComplete!";
				LevelCompleteTextObject.SetActive(true);
			}
			else if (level1 != null) {
				LevelCompleteTextObject.GetComponent<TextMeshProUGUI>().text = "Level 1\nComplete!";
				LevelCompleteTextObject.SetActive(true);
			}
			else if (level2 != null) {
				LevelCompleteTextObject.GetComponent<TextMeshProUGUI>().text = "Level 2\nComplete!";
				LevelCompleteTextObject.SetActive(true);
			}
			rainbowText.StarColorChange();
			GameObject.FindWithTag("TimerColor").GetComponent<RainbowText>().StarColorChange();
			GameObject.FindWithTag("CreditsColor").GetComponent<RainbowText>().StarColorChange();
			GameObject.FindWithTag("LivesColor").GetComponent<RainbowText>().StarColorChange();
			timerIsActive = false;
			audioSource.PlayOneShot(winAudio, 1.0f);
			state = _state.GROUNDED;
			Debug.Log("grounded final platform collision: " + other.gameObject.name + $"({Time.frameCount})");

        }


    }

	/*
	void OnCollisionStay(Collision other)
	{	
		if(other.gameObject.tag == "MovingPlatform")
		{
			Debug.Log("grounded moving platform trigger: " + other.gameObject.name + $"({Time.frameCount})");
			// Make the platform the parent of the player
			//transform.parent = other.gameObject.transform;
			// Calculate the player's position relative to the platform
			//relativePosition = transform.position - other.gameObject.transform.position;
			state = _state.GROUNDED;
		}
		// Assuming the player has a Rigidbody component
		Rigidbody playerRigidbody = GetComponent<Rigidbody>();

		//if (transform.position.y <= other.transform.position.y)
		//{
			// Player is not on top of the platform, send him back to spawn
			//transform.position = spawnPoint;
		//}
	}
	*/


	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "MovingPlatform")
		{
			// Remove the platform as the parent of the player
			transform.parent = null;
			this.onPlatform = false;
		}
	}

	void OnCollisionExit(Collision other)
	{
		if (other.gameObject.tag == "MovingPlatform" || other.gameObject.tag == "Platform" || other.gameObject.tag == "RespawnBarrier")
		{
			// Remove the platform as the parent of the player
			transform.parent = null;
			this.onPlatform = false;
		}
	}

	void UpdateScore()
	{
		// update the score UI
		scoreText.text = "Credits: " + score.ToString();
	}

	void UpdateLives()
	{
		int l = lives;
		if (powerup_current == 3)
		{
			l = 9001;
		}

		if (l == 9001)
		{
			livesText.text = l.ToString() + "♥";
		}
		else if (l == 0)
		{
			livesText.text = "";
		}
		else
		{
			livesText.text = new String('♥', l);
		}
		livesText.color = Color.red;
	}

    IEnumerator WaitForVideo(GameObject vpobj)
    {
		VideoPlayer vp = vpobj.GetComponent<VideoPlayer>();
		//Debug.Log($"({vp.frame})/({vp.frameCount})");
        while (vp.frame != ((long)vp.frameCount)-1)
        {
            yield return null;
        }
		Debug.Log("done");
		vpobj.SetActive(false);
		//Time.timeScale = 1f;
		this.level_sys.LoadLevel();
		Time.timeScale = 1f;
	}

	void Die()
	{
		// in the absence of a correct level reload, simply reload this active scene
		// TODO
		this.onPlatform = false;
		this.powerup_current = 0;
		this.death_screen.SetActive(true);

		StartCoroutine(WaitForVideo(this.death_screen));
		//this.transform.position.Set(this.transform.position.x, this.transform.position.y + 1000,this.transform.position.z);
		Time.timeScale = 0f;
	}
	
}
