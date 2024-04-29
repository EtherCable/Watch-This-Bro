using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositeMovingPlatforms : MonoBehaviour
{
    // Start is called before the first frame update
    public float Offset = 3;
    public float speed = 2;

    private float leftLimit;
    private float rightLimit;
    private Rigidbody rb;
    private bool changed;
	private float move = 0.0f;

    void Start()
    {
        leftLimit = transform.position.x - Offset;
        rightLimit = transform.position.x + Offset;
        rb = GetComponent<Rigidbody>();
        // Move the platform left
		move = -speed;
        changed = true;
    }

    void FixedUpdate()
    {
		rb.MovePosition(transform.position + Vector3.right * move * Time.deltaTime);

        // Once the left platform reaches left limit, reverse direction
        if (transform.position.x < leftLimit && changed)
        {
			move = -move;
            changed = !changed;
        }

        // Once the right platform reaches right limit, reverse direction
        if (transform.position.x > rightLimit && !changed)
        {
			move = -move;
            changed = !changed;
        }
    }
}
