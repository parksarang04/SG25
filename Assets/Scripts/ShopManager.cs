using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public Button oncartBtn;        //���� ��� ��ٱ��� ��ư ����
    public GameObject productPrefab;//��ǰâ ������ ����
    public GameObject shopPanel;    //���� �г�
    public GameObject productContent;

    public Button cartBtn;          //��ǰ ��ٱ��� ��ư ����
    public int money;               //���� �Ӵ� ����
    public TMP_Dropdown dropdown;   //���� ��� ��Ӵٿ� ��ư ����
    public Texture2D image;         //���� �̹��� ���� 
    public string productName;      //��ǰ ���� �̸� ����
    public int price;               //��ǰ ���� ���� ����
    public ProductData[] products;

    
    void Start()
    {
        Generateproduct();
    }

    public void Generateproduct()
    {
        for(int i = 0; i < products.Length; i++)    //��ǰ�� ������ŭ ��ǰâ�� �����.
        {
            GameObject productObj = Instantiate(productPrefab, productContent.transform);     //Instantiate ������� ��
            //productObj.transform.SetParent(productContent.transform);

            
        }


    }


}
