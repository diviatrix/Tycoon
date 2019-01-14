using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public float zoomSpeed; // camera zoom speed
    public float touchZoomSpeed; // 
    public float camSpeed; // camera move speed
    public float touchSpeed; // 
    public float rotSpeed; // camera move speed
    public float zLimit; // vertical camera limit

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    // private stuff
    private bool isOrtho; // is camera orthographic
    Camera cameraComponent; // just link for camera
    private Transform playerTransform; // link for player parent transform
    
    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start()
    {
        cameraComponent = GetComponentInChildren<Camera>(); // search for camera component

        // set ortho camera mode
        isOrtho = cameraComponent.orthographic;

        // set player object and transform
        playerTransform = transform.parent; 
    }

    // Update is called once per frame
    void Update()
    {
        
        // camera ortho switch
        // dunno why i need this
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerTransform.Rotate(Vector3.up, 90);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (isOrtho)
            {
                cameraComponent.orthographic = false;
                isOrtho = false;
            }
            else
            {
                cameraComponent.orthographic = true;
                isOrtho = true;
            }
        }    

        // touch controls
        
        if (Input.touchCount > 0)
        {
            if (IsPointerOverUIObject())
            {
                return;
            }

            Touch touch = Input.GetTouch(0);

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved && Input.touchCount == 1)
            {
                playerTransform.position -= playerTransform.right * touch.deltaPosition.x * touchSpeed;
                playerTransform.position -= playerTransform.forward * touch.deltaPosition.y * touchSpeed;
            }

            else if (Input.touchCount == 2)
            {
                // Store both touches.
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touch.position - touch.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touch.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                
                   // ... change the orthographic size based on the change in distance between the touches.
                cameraComponent.orthographicSize += deltaMagnitudeDiff * touchZoomSpeed;

                    // Make sure the orthographic size never drops below zero.
                cameraComponent.orthographicSize = Mathf.Max(cameraComponent.orthographicSize, 0.5f);
            }  
        }

        // mouse control

        if (!Application.isMobilePlatform)
        {
            // camera rot
            if (Input.GetMouseButton(1))
            {
                if (Application.isMobilePlatform) return;
                CameraRotationHandler();
            }    
                

            // camera dnd
            if (Input.GetMouseButton(2))
            {
                CameraMovementHandler();
            }

            CameraZoomHandler();
        }            
    }

    void CameraRotationHandler()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            cameraComponent.transform.Rotate(Input.GetAxis("Mouse Y") * -rotSpeed, 0, 0, Space.Self);
        }
        else
        {
            playerTransform.Rotate(0, Input.GetAxis("Mouse X") * rotSpeed, 0, Space.Self);
        }
    }

    void CameraMovementHandler()
    {
        playerTransform.position -= playerTransform.right * Input.GetAxis("Mouse X") * camSpeed;
        playerTransform.position -= playerTransform.forward * Input.GetAxis("Mouse Y") * camSpeed;
    }

    void CameraZoomHandler()
    {
        // camera ortho zoom
        if (isOrtho)
        {
            if (cameraComponent.orthographicSize >= zLimit)
            {
                cameraComponent.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            }

            // push to zlimit if camera is too close
            if (cameraComponent.orthographicSize < zLimit)
            {
                cameraComponent.orthographicSize = zLimit;
            }
        }
        // camera perspective zoom
        if (!isOrtho)
        {
            if (playerTransform.position.y >= zLimit)
            {
                playerTransform.position -= playerTransform.up * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            }

            // push to zlimit if camera is too close
            if (playerTransform.position.y < zLimit)
            {
                playerTransform.position = new Vector3(playerTransform.position.x, zLimit, playerTransform.position.z);
            }
        }
    }
}