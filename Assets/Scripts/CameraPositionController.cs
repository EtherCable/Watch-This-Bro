using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionController : MonoBehaviour
{
    public Transform player;
    private Vector3 offset;
    private Quaternion originalRotation;
    private Quaternion newRotation = Quaternion.Euler(10, 180, 0);
    private Vector3 newOffset = new Vector3(0, 1, 3);

    public bool isRotated = false; // Add this line to store the rotation state

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
        originalRotation = transform.rotation; // Save the original rotation
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G)) // If G key is pressed
        {
            isRotated = !isRotated; // Toggle the rotation state
        }

        if (isRotated)
        {
            transform.position = player.transform.position + newOffset; // Update the position relative to the player
            transform.rotation = newRotation;
        }
        else
        {
            transform.position = player.transform.position + offset;
            transform.rotation = originalRotation;
        }
    }
}
