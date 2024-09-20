using UnityEngine;

public class Product : MonoBehaviour
{
    public ProductData productData;

    private void Start()
    {
        if (productData != null)
        {
            Debug.Log("아이템 이름: " + productData.Name);
            Debug.Log("아이템 가격: " + productData.sellCost);
        }
        else
        {
            Debug.LogError("ProductData가 할당되지 않았습니다!");
        }
    }
}
