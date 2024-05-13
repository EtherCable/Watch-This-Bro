using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionController : MonoBehaviour
{
    // i WILL get this working even if it kills me - jordan
    public enum CamMode
    {
        CAM_DEFAULT,
        CAM_BACKWARDS,
        CAM_FREE,
        CAM_FIRSTPERSON
    }

    public Transform player;
    public float camRotationSpeed = 500.0f;
    private Vector3 offset;
    private Quaternion originalRotation;
    private Quaternion newRotation = Quaternion.Euler(10, 180, 0);
    private Vector3 newOffset = new Vector3(0, 1, 3);
    public CamMode CamState = 0; // my enums hate me and my family, 0 = default 1 = free 2 = backwards

    public bool isRotated = false; // Add this line to store the rotation state

    [SerializeField] Transform CamPos;
    private Vector3 ThirdPersonPos;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
        originalRotation = transform.rotation; // Save the original rotation
        CamState = CamMode.CAM_DEFAULT; // start with default cam
        ThirdPersonPos = CamPos.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // handle changes in state
        if (Input.GetKeyDown(KeyCode.G))
        {
            switch (CamState)
            {
                case CamMode.CAM_DEFAULT: // default
                    CamState = CamMode.CAM_FREE;
                    Debug.Log("Changed CamState to CAM_FREE");
                    transform.position = player.transform.position + offset;
                    transform.rotation = originalRotation;
                    break;
                
                case CamMode.CAM_FREE: // free
                    CamState = CamMode.CAM_BACKWARDS;
                    Debug.Log("Changed CamState to CAM_BACKWARDS");
                    transform.position = player.transform.position + newOffset; // Update position relative to player
                    transform.rotation = newRotation;
                    isRotated = true;
                    break;
                
                case CamMode.CAM_BACKWARDS: // backwards
                    CamState = CamMode.CAM_FIRSTPERSON;
                    isRotated = false;
                    transform.rotation = originalRotation;
                    CamPos.position = new Vector3(0,0,0);
                    Debug.Log("Changed CamState to CAM_FIRSTPERSON");
                    break;
                
                case CamMode.CAM_FIRSTPERSON:
                    CamState = CamMode.CAM_DEFAULT;
                    Debug.Log("Changed CamState to CAM_DEFAULT");
                    transform.position = player.transform.position + offset;
                    transform.rotation = originalRotation;
                    CamPos.position = ThirdPersonPos;
                    break;
            }
        }

        // proceed to do camera things depending on which camera state we are in
        switch (CamState)
        {
            case CamMode.CAM_DEFAULT:
                transform.position = player.transform.position + offset;
                transform.rotation = originalRotation;
                break;
            
            case CamMode.CAM_BACKWARDS:
                // transform.position = player.transform.position + newOffset; // Update position relative to player
                // transform.rotation = newRotation;
                break;
            
            case CamMode.CAM_FREE:
                if(Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
                {
                    float verticalInput = Input.GetAxis("Mouse Y") * camRotationSpeed * Time.deltaTime;
                    float horizontalInput = Input.GetAxis("Mouse X") * camRotationSpeed * Time.deltaTime;
                    transform.Rotate(Vector3.right, -verticalInput);
                    transform.Rotate(Vector3.up, horizontalInput, Space.World);
                }
                break;
            
             case CamMode.CAM_FIRSTPERSON:
                CamPos.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z + 1);
                if(Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
                {
                    float verticalInput = Input.GetAxis("Mouse Y") * camRotationSpeed * Time.deltaTime;
                    float horizontalInput = Input.GetAxis("Mouse X") * camRotationSpeed * Time.deltaTime;
                    transform.transform.Rotate(Vector3.right, -verticalInput);
                    transform.transform.Rotate(Vector3.up, horizontalInput, Space.World);
                }
                break;
        }
    }

}


