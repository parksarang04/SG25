using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ShopManager : MonoBehaviour
{
    [Header("상점패널")]
    public Button OnCartPanelButton; //오른쪽 상단 장바구니 버튼 변수
    public GameObject productPrefab;//상품창 프리팹 변수
    public GameObject shopPanel;    //상점 패널
    public GameObject productContent;
    public Image image;         //물건 이미지 변수 
    public TextMeshProUGUI productName;      //상품 물건 이름 변수
    public TextMeshProUGUI price;               //상품 물건 가격 변수
    public ProductData[] products;
    public GameObject CartPanel;

    [Header("장바구니 패널")]
    public TextMeshProUGUI PlayerMoneyText; //플레이어 돈 표시
    public Button buyButton;    // 장바구니에서 '구매'
    public Button removeButton; //장바구니에서 '지우기'
    public List<ProductData> productDatas = new List<ProductData>();
    public GameObject CartProductContent;
    public GameObject CartProductPrefab;


    void Start()
    {

        Generateproduct();
        products = Resources.LoadAll<ProductData>("");  //리소스 파일에 있는 ProductData타입을 모두 products배열에 넣는다.
       // OnCartPanelButtonClick();
        GenerateCartProduct();

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
            Button CartBtn = productObj.transform.GetChild(6).GetComponentInChildren<Button>();
            // Store a local copy of the count input field
            TMP_InputField localCount = count;
            CartBtn.onClick.AddListener(()=> CartBtnClick(count, products[i]));
            plusBtn.onClick.AddListener(() => CountUp(localCount));
            minusBtn.onClick.AddListener(() => CountDown(localCount));


            if (productObj != null && products[i] != null)
            {
                productName.text = products[i].name;               
                price.text = products[i].buyCost.ToString();
            }
        }


    }

    public void CartBtnClick(TMP_InputField count, ProductData product)
    {
        int productCount = int.Parse(count.text);   //인풋필드에 적힌 숫자 텍스트를 int로 형변환
        if(productCount > 0)
        {
            for(int i=0; i < productCount; i++)
            {
                productDatas.Add(product);
            }
        }

    }

    public void OnCartPanelButtonClick() //장바구니 버튼을 클릭했을 때 함수
    {
        CartPanel.SetActive(true); //Activ가 true면 활성화가 된다. false면 비활성화가 된다.
        GenerateCartProduct();
    }

    public void CartPanelClose()
    {
        CartPanel.SetActive(false);
    }

    public void GenerateCartProduct()
    {
        for(int i = 0; i< productDatas.Count; i++)
        {
            GameObject cartProduct = Instantiate(CartProductPrefab, CartProductContent.transform);
            TextMeshProUGUI productName = cartProduct.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            Image productImage = cartProduct.transform.GetChild(1).GetComponentInChildren<Image>();
            Button productOneRemove = cartProduct.transform.GetChild(2).GetComponentInChildren<Button>();
            Button productAllRemove = cartProduct.transform.GetChild(3).GetComponentInChildren<Button>();
            TextMeshProUGUI productText = cartProduct.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void CountUp(TMP_InputField count)
    {
        int plus = int.Parse(count.text);
        plus++;
        count.text = plus.ToString();
        Debug.Log(count.text);

    }

    public void CountDown(TMP_InputField count)
    {
        int minus = int.Parse(count.text);
        minus--;
        count.text = minus.ToString();
        Debug.Log(count.text);
    }
}
