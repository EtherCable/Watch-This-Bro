using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public float Offset = 0;
    public float speed = 0;

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
        //rb.velocity = new Vector3(speed, 0, 0);
		move = speed;
        changed = true;
    }

    void FixedUpdate()
    {
		//transform.Translate(Vector3.right * move * Time.deltaTime);
		rb.MovePosition(transform.position + Vector3.right*move*Time.deltaTime);

        if (transform.position.x > rightLimit && changed)
        {
            //rb.velocity = new Vector3(rb.velocity.x * -1, rb.velocity.y, rb.velocity.z);
			move = -move;
            changed = !changed;
        }
        
        if (transform.position.x < leftLimit && !changed)
        {
            //rb.velocity = new Vector3(rb.velocity.x * -1, rb.velocity.y, rb.velocity.z);
			move = -move;
            changed = !changed;
        }

    }
}
