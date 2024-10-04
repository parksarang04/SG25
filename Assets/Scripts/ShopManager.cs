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
   // public TextMeshProUGUI PlayerMoneyText; //플레이어 돈 표시
    public Button buyButton;    // 장바구니에서 '구매'
    public Button removeButton; //장바구니에서 '지우기'
    public List<ProductData> productDatas = new List<ProductData>();
    public GameObject CartProductContent;
    public GameObject CartProductPrefab;

    [Header("플레이어 머니")]
    public int playerMoney = 1000; // 초기 플레이어 돈 
    public TextMeshProUGUI PlayerMoneyText; // UI에서 플레이어 돈을 표시하는 텍스트


    void Start()
    {

        Generateproduct();
        products = Resources.LoadAll<ProductData>("");  //리소스 파일에 있는 ProductData타입을 모두 products배열에 넣는다.
       // OnCartPanelButtonClick();
        GenerateCartProduct();

    }

    // 장바구니 항목을 제품과 수량을 함께 관리하는 구조.
    //productDatas 리스트에서 ProductData만 저장하는 것이 아니라, 제품과 그 수량을 함께 저장하는 방식으로 변경
    public class CartItem
    {
        public ProductData product;
        public int quantity;

        public CartItem(ProductData product, int quantity)
        {
            this.product = product;
            this.quantity = quantity;
        }
    }

    public List<CartItem> cartItems = new List<CartItem>(); // 장바구니 항목 리스트


    // 플레이어 돈 UI 업데이트
    public void UpdatePlayerMoneyUI()
    {
        PlayerMoneyText.text = $"Money: {playerMoney}"; // 플레이어의 돈을 텍스트에 표시
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
        int productCount = int.Parse(count.text);

        if (productCount > 0)
        {
            // 장바구니에 이미 같은 제품이 있는지 확인
            CartItem existingItem = cartItems.Find(item => item.product.name == product.name);

            if (existingItem != null)
            {
                // 같은 제품이 있을 경우 수량만 증가
                existingItem.quantity += productCount;
            }
            else
            {
                // 새로운 제품을 장바구니에 추가
                cartItems.Add(new CartItem(product, productCount));
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
        // 기존 장바구니 항목들을 모두 제거
        foreach (Transform child in CartProductContent.transform)
        {
            Destroy(child.gameObject);
        }

        // 장바구니에 담긴 항목들을 UI에 표시
        foreach (CartItem cartItem in cartItems)
        {
            GameObject cartProduct = Instantiate(CartProductPrefab, CartProductContent.transform);
            TextMeshProUGUI productName = cartProduct.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            Image productImage = cartProduct.transform.GetChild(1).GetComponentInChildren<Image>();
            TextMeshProUGUI productQuantity = cartProduct.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>();

            // 제품 정보와 수량 표시
            productName.text = cartItem.product.name;
            productQuantity.text = $"x{cartItem.quantity}";
        }
    }


    public void OnBuyButtonClick()
    {
        int totalPrice = CalculateTotalPrice(); // 장바구니의 총 가격 계산

        // 플레이어의 돈이 총 가격보다 많거나 같을 때 구매 가능
        if (playerMoney >= totalPrice)
        {
            playerMoney -= totalPrice; // 플레이어 돈에서 총 가격 차감
            UpdatePlayerMoneyUI();     // UI 업데이트

            Debug.Log($"Items purchased for {totalPrice}. Remaining money: {playerMoney}");

            ClearCart(); // 장바구니 비우기
        }
        else
        {
            Debug.Log("아이템을 살 돈이 부족합니다.");
        }
    }

    public void ClearCart()
    {
        cartItems.Clear(); // 장바구니 비우기
        GenerateCartProduct(); // UI 업데이트 (장바구니를 다시 그려서 비워진 상태 표시)
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

    public int CalculateTotalPrice()
    {
        int totalPrice = 0;

        // 장바구니에 담긴 모든 제품의 총 가격 계산
        foreach (CartItem cartItem in cartItems)
        {
            totalPrice += cartItem.product.buyCost * cartItem.quantity; // 제품 가격 * 수량
        }

        return totalPrice;
    }


}
