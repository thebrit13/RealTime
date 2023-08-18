using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private const float MIN_CAMERA_MOVEMENT = 10;
    private const float MAX_CAMERA_MOVEMENT = 20;

    private const float SCREEN_DETECT_MIN = 25;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraMovement();
    }

    private void UpdateCameraMovement()
    {
        Vector3 movementAmt = Vector3.zero;
        //Mouse Movement
        if(Input.mousePosition.x < SCREEN_DETECT_MIN)
        {
            movementAmt.x = -MAX_CAMERA_MOVEMENT;
        }
        else if (Input.mousePosition.x > Screen.width - SCREEN_DETECT_MIN)
        {
            movementAmt.x = MAX_CAMERA_MOVEMENT;
        }
        if (Input.mousePosition.y < SCREEN_DETECT_MIN)
        {
            movementAmt.z = -MAX_CAMERA_MOVEMENT;
        }
        else if (Input.mousePosition.y > Screen.height - SCREEN_DETECT_MIN)
        {
            movementAmt.z = MAX_CAMERA_MOVEMENT;
        }


        //WASD
        if (Input.GetKey(KeyCode.W))
        {
            movementAmt.z = MAX_CAMERA_MOVEMENT;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movementAmt.z = -MAX_CAMERA_MOVEMENT;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementAmt.x = -MAX_CAMERA_MOVEMENT;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movementAmt.x = MAX_CAMERA_MOVEMENT;
        }
        
        this.transform.position += movementAmt * Time.deltaTime;
    }
}
