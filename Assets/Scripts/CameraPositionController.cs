using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionController : MonoBehaviour
{
    // i WILL get this working even if it kills me - jordan
    // public enum CamMode
    // {
    //     CAM_DEFAULT,
    //     CAM_BACKWARDS,
    //     CAM_FREE
    // }

    public Transform player;
    public float camRotationSpeed = 500.0f;
    private Vector3 offset;
    private Quaternion originalRotation;
    private Quaternion newRotation = Quaternion.Euler(10, 180, 0);
    private Vector3 newOffset = new Vector3(0, 1, 3);
    public int CamState = 0; // my enums hate me and my family, 0 = default 1 = free 2 = backwards

    // following commented out in favor of a State implementation
    public bool isRotated = false; // Add this line to store the rotation state

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
        originalRotation = transform.rotation; // Save the original rotation
        CamState = 0; // start with default cam
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // handle changes in state
        if (Input.GetKeyDown(KeyCode.G))
        {
            switch (CamState)
            {
                case 0: // default
                    CamState = 1;
                    Debug.Log("Changed CamState to CAM_FREE");
                    break;
                
                case 1: // free
                    CamState = 2;
                    Debug.Log("Changed CamState to CAM_BACKWARDS");
                    transform.position = player.transform.position + offset;
                    transform.rotation = originalRotation;
                    break;
                
                case 2: // backwards
                    CamState = 0;
                    Debug.Log("Changed CamState to CAM_DEFAULT");
                    break;
            }
        }

        // proceed to do camera things depending on which camera state we are in
        switch (CamState)
        {
            case 0:
                transform.position = player.transform.position + offset;
                transform.rotation = originalRotation;
                break;
            
            case 1:
                transform.position = player.transform.position + newOffset; // Update position relative to player
                transform.rotation = newRotation;
                break;
            
            case 2:
                if(Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
                {
                    float verticalInput = Input.GetAxis("Mouse Y") * camRotationSpeed * Time.deltaTime;
                    float horizontalInput = Input.GetAxis("Mouse X") * camRotationSpeed * Time.deltaTime;
                    transform.Rotate(Vector3.right, -verticalInput);
                    transform.Rotate(Vector3.up, horizontalInput, Space.World);
                }
                break;
        }
    }

}


