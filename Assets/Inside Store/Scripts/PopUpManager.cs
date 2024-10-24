using UnityEngine;
using UnityEngine.UI; // Unity의 UI 컴포넌트를 사용하려면 필요함
using TMPro;

public class PopUpManager : MonoBehaviour
{
    public GameObject popupPanel; // 팝업 패널
    public TextMeshProUGUI popupMessage;     // 팝업에 표시할 메시지
    public int RandomNum = 0;

    // 팝업을 열 때 호출하는 함수
    public void OpenPopUp(string message)
    {
        popupPanel.SetActive(true); // 팝업 패널을 활성화
        Debug.Log("팝업 열기 시도"); // 디버그 메시지 추가
        popupMessage.text = message; // 메시지를 설정
    }

    // 팝업을 닫을 때 호출하는 함수
    public void ClosePopUp()
    {
        popupPanel.SetActive(false); // 팝업 패널을 비활성화
    }

    // 확인 버튼 클릭 시 호출되는 함수
    public void OnConfirmButtonClick()
    {
        Debug.Log("Confirm button clicked");
        ClosePopUp(); // 팝업 닫기
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
        string vvvvv = "안녕하세요. SG25 본사 입니다. 현상수배범이 있으니 발견 시 신고해주세요.";
        OpenPopUp(vvvvv);
    }
    /*public void Messageqewr()
    {
        string vvvvv = "";
        OpenPopUp(vvvvv);
    }
    public void Messageddfa()
    {
        string vvvvv = "박사랑 똥";
        OpenPopUp(vvvvv);
    }*/
}