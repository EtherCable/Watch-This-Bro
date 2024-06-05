using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public float camMin = -45.0f;
    public float camMax = 50.0f;
    private Vector3 offset;
    private Quaternion originalRotation;
    private Quaternion newRotation = Quaternion.Euler(10, 180, 0);
    private Vector3 newOffset = new Vector3(0, 1, 3);
    public CamMode CamState = CamMode.CAM_DEFAULT; // my enums hate me and my family, 0 = default 1 = free 2 = backwards

    public bool isRotated = false; // Add this line to store the rotation state

    public TextMeshProUGUI camInfo;
    public Camera FPSCam;
    public Camera THIRDCam;

    // [SerializeField] Transform CamPos;
    private Vector3 ThirdPersonPos;
    private float pitch = 0.0f;
    private float yaw = 0.0f;
    bool textOn = true;
    bool escOn = true;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
        originalRotation = transform.rotation; // Save the original rotation
        SetThirdPerson();
        GameObject level0 = GameObject.Find("Level0");
        if (level0 == null)
        {
            GameObject.Find("POVtext").SetActive(false);
            GameObject.Find("EsctoPause").SetActive(false);
        }
    }
    
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.G) && textOn == true) {
        GameObject.Find("POVtext").SetActive(false);
        textOn = false;
       }
       if (Input.GetKeyDown(KeyCode.Escape) && escOn == true) {
        if (GameObject.Find("EsctoPause") != null) {
            GameObject.Find("EsctoPause").SetActive(false);
            escOn = false;
        }
       }
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
                        StartCoroutine(ShowMessage("Mode: Free Rotation"));
                        Debug.Log("Changed CamState to CAM_FREE");
                        // transform.position = player.transform.position + offset;
                        transform.rotation = originalRotation;
                        break;
                    
                    case CamMode.CAM_FREE: // free
                        CamState = CamMode.CAM_BACKWARDS;
                        StartCoroutine(ShowMessage("Mode: Challenge"));
                        Debug.Log("Changed CamState to CAM_BACKWARDS");
                        // transform.position = player.transform.position + newOffset; // Update position relative to player
                        transform.rotation = newRotation;
                        isRotated = true;
                        break;
                    
                    case CamMode.CAM_BACKWARDS: // backwards
                        CamState = CamMode.CAM_FIRSTPERSON;
                        StartCoroutine(ShowMessage("Mode: First Person"));
                        isRotated = false;
                        transform.rotation = originalRotation;
                        Debug.Log("Changed CamState to CAM_FIRSTPERSON");
                        SetFirstPerson();
                        break;
                    
                    case CamMode.CAM_FIRSTPERSON:
                        CamState = CamMode.CAM_DEFAULT;
                        StartCoroutine(ShowMessage("Mode: Third Person"));
                        Debug.Log("Changed CamState to CAM_DEFAULT");
                        transform.rotation = originalRotation;
                        SetThirdPerson();
                        break;
            }
        }
        

        // proceed to do camera things depending on which camera state we are in
        switch (CamState)
        {
            case CamMode.CAM_DEFAULT:
                // transform.position = player.transform.position + offset;
                // transform.rotation = originalRotation;
                break;
            
            case CamMode.CAM_BACKWARDS:
                // transform.position = player.transform.position + newOffset; // Update position relative to player
                // transform.rotation = newRotation;
                break;
            
            case CamMode.CAM_FREE:
                // if(Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
                // {
                    yaw += Input.GetAxis("Mouse X") * camRotationSpeed * Time.deltaTime;
                    pitch += Input.GetAxis("Mouse Y") * camRotationSpeed * Time.deltaTime;
                    pitch = Mathf.Clamp(pitch, camMin, camMax);
                    transform.eulerAngles = new Vector3 (-pitch, yaw, 0.0f);
                // }
                
                break;
            
             case CamMode.CAM_FIRSTPERSON:
                // CamPos.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z + 1);
                yaw += Input.GetAxis("Mouse X") * camRotationSpeed * Time.deltaTime;
                pitch += Input.GetAxis("Mouse Y") * camRotationSpeed * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, camMin, camMax);
                transform.eulerAngles = new Vector3 (-pitch, yaw, 0.0f);
                break;
        }
    }

    void SetFirstPerson()
    {
        FPSCam.enabled = true;
        THIRDCam.enabled = false;
    }

    
    void SetThirdPerson()
    {
        THIRDCam.enabled = true;
        FPSCam.enabled = false;
    }

    IEnumerator ShowMessage(string message)
    {
        camInfo.text = message;
        yield return new WaitForSeconds(3); // wait for 3 seconds
        camInfo.text = "";
    }
}


