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

    void Start()
    {
        leftLimit = transform.position.x - Offset;
        rightLimit = transform.position.x + Offset;
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(speed, 0, 0);
        changed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > rightLimit && changed)
        {
            rb.velocity = new Vector3(rb.velocity.x * -1, rb.velocity.y, rb.velocity.z);
            changed = !changed;
        }
        
        if (transform.position.x < leftLimit && !changed)
        {
            rb.velocity = new Vector3(rb.velocity.x * -1, rb.velocity.y, rb.velocity.z);
            changed = !changed;
        }

    }
}
