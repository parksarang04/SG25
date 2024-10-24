using UnityEngine;
using UnityEngine.UI; // Unity�� UI ������Ʈ�� ����Ϸ��� �ʿ���
using TMPro;

public class PopUpManager : MonoBehaviour
{
    public GameObject popupPanel; // �˾� �г�
    public TextMeshProUGUI popupMessage;     // �˾��� ǥ���� �޽���
    public int RandomNum = 0;

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

    // Ȯ�� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnConfirmButtonClick()
    {
        Debug.Log("Confirm button clicked");
        ClosePopUp(); // �˾� �ݱ�
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RandomNum = 1;
        }

        switch(RandomNum)
        {
            case 1:
                MessageWanted();
                break;

            /*case 2:
                Messageqewr();
                break;
            case 3:
                Messageddfa();
                break;*/
        }

    }
    public void MessageWanted()
    {
        string vvvvv = "�ȳ��ϼ���. SG25 ���� �Դϴ�. ���������� ������ �߰� �� �Ű����ּ���.";
        OpenPopUp(vvvvv);
    }
    /*public void Messageqewr()
    {
        string vvvvv = "";
        OpenPopUp(vvvvv);
    }
    public void Messageddfa()
    {
        string vvvvv = "�ڻ�� ��";
        OpenPopUp(vvvvv);
    }*/
}