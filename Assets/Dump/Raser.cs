using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raser : MonoBehaviour
{
    private Camera playerCam;
    private float MaxDistance = 5f;
    public Transform holdPos;
    private GameObject holdObj;
    private Collider holdObjCollider;
    private Quaternion fixedRot = Quaternion.Euler(0, 0, 0);    

    void Start()
    {
        
        playerCam = Camera.main;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(holdObj == null)
            {
                PickUp();
            }
            else
            {
                Drop();
            }
        }
    }

    void PickUp()
    {
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, MaxDistance))
        {
            if(hit.collider.gameObject.CompareTag("Product"))
            {
                holdObj = hit.collider.gameObject;
                holdObjCollider = holdObj.GetComponent<Collider>();

                Rigidbody rb = holdObj.GetComponent<Rigidbody>();

                if(rb != null)
                {
                    rb.isKinematic = true;
                    holdObjCollider.enabled = false;
                    holdObj.transform.SetParent(holdPos);
                    holdObj.transform.localPosition = Vector3.zero;
                    holdObj.transform.localRotation = fixedRot;
                }
            }
        }
    }

    void Drop()
    {
        if(holdObj != null)
        {
            Rigidbody rb = holdObj.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.isKinematic = false;
                holdObjCollider.enabled = true;
                holdObj.transform.SetParent(null);
            }

            holdObj = null;
            holdObjCollider = null;
        }
    }
}
