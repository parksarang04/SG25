using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDoorCtrl : MonoBehaviour
{
    public bool isOpen = false;
    public float doorOpenAngle = 90f;
    public float doorCloseAngle = 0f;
    public float smoot = 2f;

    public void ChangeDoorState()
    {
        isOpen = !isOpen;
    }

    public void OpenDoor()
    {
        isOpen = true;
        Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoot * Time.deltaTime);
    }
    public void CloseDoor()
    {
        isOpen = false;
        Quaternion targetRotation2 = Quaternion.Euler(0, doorCloseAngle, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation2, smoot * Time.deltaTime);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ChangeDoorState();
        }
        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
}
