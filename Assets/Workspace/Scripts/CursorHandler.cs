using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    private bool isCursorHidden = true;

    void Start()
    {
        // 게임 시작 시 마우스를 숨깁니다.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // M 키를 누르면 마우스 커서의 상태를 전환합니다.
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        isCursorHidden = !isCursorHidden;

        if (isCursorHidden)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

}
