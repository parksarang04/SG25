using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    private bool isCursorHidden = true;

    void Start()
    {
        // ���� ���� �� ���콺�� ����ϴ�.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // M Ű�� ������ ���콺 Ŀ���� ���¸� ��ȯ�մϴ�.
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
