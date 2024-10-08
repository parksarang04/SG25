using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float turnSpeedx = 2.0f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    { 
        Move();

        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeedx;
        transform.Rotate(0, yRotateSize, 0);
    }

    private void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;
        
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        transform.Translate(move, Space.World);
    }
}
