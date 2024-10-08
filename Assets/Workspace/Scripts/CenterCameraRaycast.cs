using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCameraRaycast : MonoBehaviour
{
    public Camera targetCamera;
    public float raycastRange = 100f; // 레이캐스트의 최대 거리

    public Outline currentOutline;


    void Update()
    {
        PerformRaycast();
        PushItem();
        PopItem();
    }

    void PerformRaycast()
    {
        Ray ray = targetCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // 레이캐스트가 충돌한 물체가 있는지 확인합니다.
        if (Physics.Raycast(ray, out hit, raycastRange))
        {
            // 물체를 감지하면 해당 물체의 이름을 로그로 출력합니다.
            if (hit.collider.gameObject.tag == "Item")
            {
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
    public void PushItem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = targetCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            // 레이캐스트가 충돌한 물체가 있는지 확인합니다.
            if (Physics.Raycast(ray, out hit, raycastRange))
            {
                // 물체를 감지하면 해당 물체의 이름을 로그로 출력합니다.
                if (hit.collider.gameObject.tag == "Item")
                {
                    var shelf = hit.collider.gameObject.GetComponent<Shelf>();
                    if (shelf != null)
                    {
                        shelf.PushItem();
                    }

                }
            }

        }
    }
    public void PopItem()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = targetCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            // 레이캐스트가 충돌한 물체가 있는지 확인합니다.
            if (Physics.Raycast(ray, out hit, raycastRange))
            {
                // 물체를 감지하면 해당 물체의 이름을 로그로 출력합니다.
                if (hit.collider.gameObject.tag == "Item")
                {
                    var shelf = hit.collider.gameObject.GetComponent<Shelf>();
                    if (shelf != null)
                    {
                        shelf.PopItem();
                    }

                }
            }

        }
    }
}
