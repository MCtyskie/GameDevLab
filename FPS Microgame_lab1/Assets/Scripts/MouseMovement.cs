using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSens = 100f;
    public Transform playerBody;
    float vRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        //horizontal rotation of whole player
        playerBody.Rotate(Vector3.up * mouseX);
        //vertical rotation of camera <-90 ; 90>
        vRotation -= mouseY;
        vRotation = Mathf.Clamp(vRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(vRotation, 0f, 0f);
    }
}
