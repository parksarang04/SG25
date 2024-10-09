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
    }

    void PerformRaycast()
    {
        Ray ray = targetCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // 레이캐스트가 충돌한 물체가 있는지 확인합니다.
        if (Physics.Raycast(ray, out hit, raycastRange))
        {
            Outline newOutline = hit.collider.gameObject.GetComponent<Outline>();

            // 물체를 감지하면 해당 물체의 이름을 로그로 출력합니다.
            if (hit.collider.gameObject.tag == "Item")
            {
                Debug.Log("Hit Object: " + hit.collider.gameObject.name);
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.CompareTag("Door"))
                {
                    TestDoorCtrl hitDoor = hit.collider.GetComponent<TestDoorCtrl>();
                    hitDoor.ChangeDoorState();
                }
            }
            if (currentOutline != newOutline)
            {
                if (currentOutline != null)
                {
                    currentOutline.OutlineWidth = 0; // 이전 아웃라인 해제
                    currentOutline = null;
                }

                currentOutline = newOutline;

                if (currentOutline != null)
                {
                    currentOutline.OutlineWidth = 10; // 새로운 아웃라인 적용
                }   
            }
        }
        
    }

}
