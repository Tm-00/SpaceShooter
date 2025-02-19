using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerlook : MonoBehaviour
{
    public Camera Cam;
    private float xRotation = 0f;

    public float xSensitivity = 50f;
    public float ySensitivity = 50f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        //calculate camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -200f, 200f);
        // apply this to our camera's transform.
        Cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //rotate camera either left or right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}
