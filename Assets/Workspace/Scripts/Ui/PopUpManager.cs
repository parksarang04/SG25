using UnityEngine;
using UnityEngine.UI; // Unity�� UI ������Ʈ�� ����Ϸ��� �ʿ���
using TMPro;

public class PopUpManager : MonoBehaviour
{
    public GameObject popupPanel; // �˾� �г�
    public TextMeshProUGUI popupMessage; // �˾��� ǥ���� �޽���

    // �˾��� �� �� ȣ���ϴ� �Լ�
    public void OpenPopUp(string message)
    {
        popupPanel.SetActive(true); // �˾� �г��� Ȱ��ȭ
        Debug.Log("�˾� ���� �õ�"); // ����� �޽��� �߰�
        popupMessage.text = message; // �޽����� ����
    }

    // �˾��� ���� �� ȣ���ϴ� �Լ�
    public void ClosePopUp()
    {
        popupPanel.SetActive(false); // �˾� �г��� ��Ȱ��ȭ
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // 'P' Ű�� ������ ��
        {
            if (popupPanel.activeSelf) // �˾��� ���� �ִٸ�
            {
                ClosePopUp(); // �˾�â �ݱ�
            }
            else // �˾��� ���� �ִٸ�
            {
                MessageWanted(); // �˾�â ����
            }
        }
    }

    public void MessageWanted()
    {
        string vvvvv = "�ȳ��ϼ���. SG25 ���� �Դϴ�. ���������� ������ �߰� �� �Ű����ּ���.";
        OpenPopUp(vvvvv);
    }
}
