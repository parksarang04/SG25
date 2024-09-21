using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckoutSystem : MonoBehaviour
{
    public Text totalPriceText;
    private List<Product> selectedItems = new List<Product>();
    private int totalPrice = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))    // 좌클릭으로 아이템 선택
        {
            SelectAndRemoveItem();
        }
    }

    void SelectAndRemoveItem()
    {
        // 마우스 클릭 위치에서 Ray를 발사해 아이템과 충돌 감지
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // 충돌한 오브젝트에서 Product 컴포넌트를 가져옴
            Product product = hit.collider.GetComponent<Product>();

            if (product != null && product.product != null)
            {
                // 아이템의 가격을 totalPrice에 추가
                totalPrice += product.product.sellCost;

                // 총 가격 UI 업데이트
                UpdateTotalPrice();

                // 아이템을 클릭 시 제거
                Destroy(product.gameObject);

                // 디버그 로그로 가격 정보 출력
                Debug.Log("선택한 아이템: " + product.product.Name);
                Debug.Log("아이템 가격: " + product.product.sellCost + "원");
                Debug.Log("현재 총합 가격: " + totalPrice + "원");
            }
        }
    }

    // UI Text에 총 가격을 업데이트하는 함수
    void UpdateTotalPrice()
    {
        if (totalPriceText != null)
        {
            totalPriceText.text = "총합 가격: " + totalPrice.ToString() + "원";
        }
        else
        {
            Debug.LogError("totalPriceText가 UI에 연결되지 않았습니다.");
        }
    }
}
