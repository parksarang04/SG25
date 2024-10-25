using UnityEngine;
using UnityEngine.UI; // Unity의 UI 컴포넌트를 사용하려면 필요함
using TMPro;

public class PopUpManager : MonoBehaviour
{
    public GameObject popupPanel; // 팝업 패널
    public TextMeshProUGUI popupMessage; // 팝업에 표시할 메시지

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // 'P' 키를 눌렀을 때
        {
            if (popupPanel.activeSelf) // 팝업이 열려 있다면
            {
                ClosePopUp(); // 팝업창 닫기
            }
            else // 팝업이 닫혀 있다면
            {
                MessageWanted(); // 팝업창 열기
            }
        }
    }

    public void MessageWanted()
    {
        string vvvvv = "안녕하세요. SG25 본사 입니다. 현상수배범이 있으니 발견 시 신고해주세요.";
        OpenPopUp(vvvvv);
    }
}
