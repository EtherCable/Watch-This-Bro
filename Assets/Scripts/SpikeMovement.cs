using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    public float Offset = 1;
    public float speed = 2;
    public float pauseTime = 2; // Time to pause in seconds

    private float originalPosition;
    private float lowerLimit;
    private Rigidbody rb;
    private bool movingDown;

    void Start()
    {
        originalPosition = transform.position.y;
        lowerLimit = transform.position.y - Offset;
        rb = GetComponent<Rigidbody>();
        movingDown = true;
    }

    void FixedUpdate()
    {
        // Get the parent object's transform
        Transform parentTransform = transform.parent;

        if (movingDown)
        {
            // Move horizontally with the parent
            transform.position = new Vector3(parentTransform.position.x, transform.position.y, transform.position.z);
            
            // Move down
            rb.MovePosition(transform.position - Vector3.up * speed * Time.deltaTime);
            
            if (transform.position.y < lowerLimit)
            {
                StartCoroutine(PauseBeforeMoving());
                movingDown = false;
            }
        }
        else
        {
            // Move horizontally with the parent
            transform.position = new Vector3(parentTransform.position.x, transform.position.y, transform.position.z);
            
            // Move up
            rb.MovePosition(transform.position + Vector3.up * speed * Time.deltaTime);
            
            if (transform.position.y >= originalPosition)
            {
                StartCoroutine(PauseBeforeMoving());
                movingDown = true;
            }
        }
    }


    IEnumerator PauseBeforeMoving()
    {
        float currentSpeed = speed;
        speed = 0; // Stop the platform
        yield return new WaitForSeconds(pauseTime); // Wait for pauseTime seconds
        speed = currentSpeed; // Resume movement
    }
}
