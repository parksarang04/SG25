using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ShopManager : MonoBehaviour
{
    public Button oncartBtn;        //���� ��� ��ٱ��� ��ư ����
    public GameObject productPrefab;//��ǰâ ������ ����
    public GameObject shopPanel;    //���� �г�
    public GameObject productContent;

    public Button cartBtn;          //��ǰ ��ٱ��� ��ư ����
    //public TMP_Text money;               //���� �Ӵ� ����
    public TMP_Dropdown dropdown;   //���� ��� ��Ӵٿ� ��ư ����
    //public Image image;         //���� �̹��� ���� 
    //public TMP_Text productName;      //��ǰ ���� �̸� ����
    //public TMP_Text price;               //��ǰ ���� ���� ����
    public ProductData[] products;
    

    void Start()
    {
        Generateproduct();
        products = Resources.LoadAll<ProductData>("");
    }

    public void Generateproduct()
    {
        for (int i = 0; i < products.Length; i++)    //��ǰ�� ������ŭ ��ǰâ�� �����.
        {
            GameObject productObj = Instantiate(productPrefab, productContent.transform);     //Instantiate ������� ��
            //productObj.transform.SetParent(productContent.transform);
            TextMeshProUGUI productName = productObj.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            Image productimage = productObj.transform.GetChild(1).GetComponentInChildren<Image>();
            TextMeshProUGUI productprice = productObj.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
            if (productObj != null && products[i] != null)
            {
                productName.text = products[i] .name;
                productimage = products[i].image;
                productprice.text= products[i].buyCost.ToString(); //buyCost(int��),productprice(text��) ����ȯToString()
            }

               
        }


    }
    public void OncartBtnClick(ProductData product)
    {
        
    }


}
