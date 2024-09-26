using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ShopManager : MonoBehaviour
{
    public Button oncartBtn;        //왼쪽 상단 장바구니 버튼 변수
    public GameObject productPrefab;//상품창 프리팹 변수
    public GameObject shopPanel;    //상점 패널
    public GameObject productContent;

    public Button cartBtn;          //물품 장바구니 버튼 변수
    //public TMP_Text money;               //게임 머니 변수
    public TMP_Dropdown dropdown;   //물건 담는 드롭다운 버튼 변수
    public Image image;         //물건 이미지 변수 
    public TextMeshProUGUI productName;      //상품 물건 이름 변수
    public TextMeshProUGUI price;               //상품 물건 가격 변수
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
        for (int i = 0; i < products.Length; i++)
        {
            GameObject productObj = Instantiate(productPrefab, productContent.transform);

            // Get references to the components
            TextMeshProUGUI productName = productObj.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            Image image = productObj.transform.GetChild(1).GetComponentInChildren<Image>();
            TextMeshProUGUI price = productObj.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
            TMP_InputField count = productObj.transform.GetChild(3).GetComponentInChildren<TMP_InputField>();
            count.text = "1";

            Button plusBtn = productObj.transform.GetChild(4).GetComponentInChildren<Button>();
            Button minusBtn = productObj.transform.GetChild(5).GetComponentInChildren<Button>();

            // Store a local copy of the count input field
            TMP_InputField localCount = count;

            plusBtn.onClick.AddListener(() => CountUp(localCount));
           
            if (productObj != null && products[i] != null)
            {
                productName.text = products[i].name;               
                price.text = products[i].buyCost.ToString();
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
