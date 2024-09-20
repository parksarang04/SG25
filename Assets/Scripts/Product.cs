using UnityEngine;

public class Product : MonoBehaviour
{
    public ProductData productData;

    private void Start()
    {
        if (productData != null)
        {
            Debug.Log("������ �̸�: " + productData.Name);
            Debug.Log("������ ����: " + productData.sellCost);
        }
        else
        {
            Debug.LogError("ProductData�� �Ҵ���� �ʾҽ��ϴ�!");
        }
    }
}
