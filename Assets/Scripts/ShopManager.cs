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
    public Image image;         //���� �̹��� ���� 
    public TextMeshProUGUI productName;      //��ǰ ���� �̸� ����
    public TextMeshProUGUI price;               //��ǰ ���� ���� ����
    public ProductData[] products;
    private Button minusBtn;
    private Button plusBtn;
    private TMP_InputField count;


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
             productName = productObj.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            image = productObj.transform.GetChild(1).GetComponentInChildren<Image>();
            price = productObj.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
            count = productObj.transform.GetChild(3).GetComponentInChildren<TMP_InputField>();
            count.text = "1";
            plusBtn = productObj.transform.GetChild(4).GetComponentInChildren<Button>();
            plusBtn.onClick.AddListener(() => CountUp(count));
            minusBtn = productObj.transform.GetChild(5).GetComponentInChildren<Button>();

            if (productObj != null && products[i] != null)
            {
                productName.text = products[i].name;
                image = products[i].image;
                price.text = products[i].buyCost.ToString(); //buyCost(int��),productprice(text��) ����ȯToString()
            }

        }


    }
    public void OncartBtnClick(ProductData product)
    {
        
    }

    public void CountUp(TMP_InputField count)
    {
        int plus = int.Parse(count.text);
        plus++;
        count.text = plus.ToString();
        Debug.Log(count.text);

    }

    public void CountDown()
    {
        int minus = int.Parse(count.text);
        minus--;
        count.text = minus.ToString();
        Debug.Log(count.text);
    }
}
