using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCameraRaycast : MonoBehaviour
{
    public Camera targetCamera;
    public float raycastRange = 100f; // ����ĳ��Ʈ�� �ִ� �Ÿ�

    public Outline currentOutline;


    void Update()
    {
        PerformRaycast();
    }

    void PerformRaycast()
    {
        Ray ray = targetCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // ����ĳ��Ʈ�� �浹�� ��ü�� �ִ��� Ȯ���մϴ�.
        if (Physics.Raycast(ray, out hit, raycastRange))
        {
            // ��ü�� �����ϸ� �ش� ��ü�� �̸��� �α׷� ����մϴ�.
            if (hit.collider.gameObject.tag == "Item")
            {
                Debug.Log("Hit Object: " + hit.collider.gameObject.name);

                if (currentOutline != null)
                {
                    currentOutline.OutlineWidth = 0;
                    currentOutline = null;
                }
                currentOutline = hit.collider.gameObject.GetComponent<Outline>();
                currentOutline.OutlineWidth = 10;
            }
            else
            {
                if (currentOutline != null)
                {
                    currentOutline.OutlineWidth = 0;
                    currentOutline = null;
                }
            }
        }
        
    }

}
