using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{   
    public Transform headTransform;
    public Transform cameraTransform;
    public float turnSpeedy = 2.0f;
    private float cameraXRotation = 0.0f;
   
    
    void Start()
    {
        
    }

    
    void Update()
    {      
        float xRotateSize = Input.GetAxis("Mouse Y") * turnSpeedy;
        cameraXRotation -= xRotateSize;
        cameraXRotation = Mathf.Clamp(cameraXRotation, -40, 40);

       
        cameraTransform.localRotation = Quaternion.Euler(cameraXRotation, 0, 0);
        headTransform.localRotation = Quaternion.Euler(cameraXRotation, 0, 0);        
    }
}
