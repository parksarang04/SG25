using UnityEngine;
using UnityEngine.UI; // Unity�� UI ������Ʈ�� ����Ϸ��� �ʿ���

public class PopUpManager : MonoBehaviour
{
    public GameObject popupPanel; // �˾� �г�
    public Text popupMessage;     // �˾��� ǥ���� �޽���

    // �˾��� �� �� ȣ���ϴ� �Լ�
    public void OpenPopUp(string message)
    {
        popupPanel.SetActive(true); // �˾� �г��� Ȱ��ȭ
        Debug.Log("�˾� ���� �õ�"); // ����� �޽��� �߰�
        //popupMessage.text = message; // �޽����� ����
    }

    // �˾��� ���� �� ȣ���ϴ� �Լ�
    public void ClosePopUp()
    {
        popupPanel.SetActive(false); // �˾� �г��� ��Ȱ��ȭ
    }

    // Ȯ�� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnConfirmButtonClick()
    {
        Debug.Log("Confirm button clicked");
        ClosePopUp(); // �˾� �ݱ�
    }

    /* ��� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnCancelButtonClick()
    {
        Debug.Log("Cancel button clicked");
        ClosePopUp(); // �˾� �ݱ�
    }*/
}