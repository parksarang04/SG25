using UnityEngine;
using UnityEngine.UI; // Unity의 UI 컴포넌트를 사용하려면 필요함

public class PopUpManager : MonoBehaviour
{
    public GameObject popupPanel; // 팝업 패널
    public Text popupMessage;     // 팝업에 표시할 메시지

    // 팝업을 열 때 호출하는 함수
    public void OpenPopUp(string message)
    {
        popupPanel.SetActive(true); // 팝업 패널을 활성화
        Debug.Log("팝업 열기 시도"); // 디버그 메시지 추가
        //popupMessage.text = message; // 메시지를 설정
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

    /* 취소 버튼 클릭 시 호출되는 함수
    public void OnCancelButtonClick()
    {
        Debug.Log("Cancel button clicked");
        ClosePopUp(); // 팝업 닫기
    }*/
}