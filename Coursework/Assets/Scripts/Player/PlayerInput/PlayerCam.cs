using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{

    // will control x/y sensitivity for camera
    public float sensX;
    public float sensY;

    // x/y rotation of camera
    float xRotation;
    float yRotation;

    // will track camera movement
    public Transform orientation;


    public void Start()
    {
        //locks the cursor to the game environment
        Cursor.lockState = CursorLockMode.Locked;

        //hides the cursor in game environment
        Cursor.visible = false;
    }

    private void Update()
    {
        // gets the x and y axis input from the mouse and assigns it to a variable
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;

        //stops the camera from being able to look too far up or down
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotates the camera and orientation
        // rotates camera along both axis
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        // rotates player along y axis
        orientation.rotation = Quaternion.Euler(0, yRotation, 0); 
    }
}
