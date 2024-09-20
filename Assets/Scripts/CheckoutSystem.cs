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
        if (Input.GetMouseButtonDown(0))    // ��Ŭ������ ������ ����
        {
            SelectAndRemoveItem();
        }
    }

    void SelectAndRemoveItem()
    {
        // ���콺 Ŭ�� ��ġ���� Ray�� �߻��� �����۰� �浹 ����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // �浹�� ������Ʈ���� Product ������Ʈ�� ������
            Product product = hit.collider.GetComponent<Product>();

            if (product != null && product.productData != null)
            {
                // �������� ������ totalPrice�� �߰�
                totalPrice += product.productData.sellCost;

                // �� ���� UI ������Ʈ
                UpdateTotalPrice();

                // �������� Ŭ�� �� ����
                Destroy(product.gameObject);

                // ����� �α׷� ���� ���� ���
                Debug.Log("������ ������: " + product.productData.Name);
                Debug.Log("������ ����: " + product.productData.sellCost + "��");
                Debug.Log("���� ���� ����: " + totalPrice + "��");
            }
        }
    }

    // UI Text�� �� ������ ������Ʈ�ϴ� �Լ�
    void UpdateTotalPrice()
    {
        if (totalPriceText != null)
        {
            totalPriceText.text = "���� ����: " + totalPrice.ToString() + "��";
        }
        else
        {
            Debug.LogError("totalPriceText�� UI�� ������� �ʾҽ��ϴ�.");
        }
    }
}
