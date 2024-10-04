using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ShopManager : MonoBehaviour
{
    [Header("�����г�")]
    public Button OnCartPanelButton; //������ ��� ��ٱ��� ��ư ����
    public GameObject productPrefab;//��ǰâ ������ ����
    public GameObject shopPanel;    //���� �г�
    public GameObject productContent;
    public Image image;         //���� �̹��� ���� 
    public TextMeshProUGUI productName;      //��ǰ ���� �̸� ����
    public TextMeshProUGUI price;               //��ǰ ���� ���� ����
    public ProductData[] products;
    public GameObject CartPanel;

    [Header("��ٱ��� �г�")]
   // public TextMeshProUGUI PlayerMoneyText; //�÷��̾� �� ǥ��
    public Button buyButton;    // ��ٱ��Ͽ��� '����'
    public Button removeButton; //��ٱ��Ͽ��� '�����'
    public List<ProductData> productDatas = new List<ProductData>();
    public GameObject CartProductContent;
    public GameObject CartProductPrefab;

    [Header("�÷��̾� �Ӵ�")]
    public int playerMoney = 1000; // �ʱ� �÷��̾� �� 
    public TextMeshProUGUI PlayerMoneyText; // UI���� �÷��̾� ���� ǥ���ϴ� �ؽ�Ʈ


    void Start()
    {

        Generateproduct();
        products = Resources.LoadAll<ProductData>("");  //���ҽ� ���Ͽ� �ִ� ProductDataŸ���� ��� products�迭�� �ִ´�.
       // OnCartPanelButtonClick();
        GenerateCartProduct();

    }

    // ��ٱ��� �׸��� ��ǰ�� ������ �Բ� �����ϴ� ����.
    //productDatas ����Ʈ���� ProductData�� �����ϴ� ���� �ƴ϶�, ��ǰ�� �� ������ �Բ� �����ϴ� ������� ����
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

    public List<CartItem> cartItems = new List<CartItem>(); // ��ٱ��� �׸� ����Ʈ


    // �÷��̾� �� UI ������Ʈ
    public void UpdatePlayerMoneyUI()
    {
        PlayerMoneyText.text = $"Money: {playerMoney}"; // �÷��̾��� ���� �ؽ�Ʈ�� ǥ��
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
            // ��ٱ��Ͽ� �̹� ���� ��ǰ�� �ִ��� Ȯ��
            CartItem existingItem = cartItems.Find(item => item.product.name == product.name);

            if (existingItem != null)
            {
                // ���� ��ǰ�� ���� ��� ������ ����
                existingItem.quantity += productCount;
            }
            else
            {
                // ���ο� ��ǰ�� ��ٱ��Ͽ� �߰�
                cartItems.Add(new CartItem(product, productCount));
            }
        }
    }


    public void OnCartPanelButtonClick() //��ٱ��� ��ư�� Ŭ������ �� �Լ�
    {
        CartPanel.SetActive(true); //Activ�� true�� Ȱ��ȭ�� �ȴ�. false�� ��Ȱ��ȭ�� �ȴ�.
        GenerateCartProduct();
    }

    public void CartPanelClose()
    {
        CartPanel.SetActive(false);
    }

    public void GenerateCartProduct()
    {
        // ���� ��ٱ��� �׸���� ��� ����
        foreach (Transform child in CartProductContent.transform)
        {
            Destroy(child.gameObject);
        }

        // ��ٱ��Ͽ� ��� �׸���� UI�� ǥ��
        foreach (CartItem cartItem in cartItems)
        {
            GameObject cartProduct = Instantiate(CartProductPrefab, CartProductContent.transform);
            TextMeshProUGUI productName = cartProduct.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            Image productImage = cartProduct.transform.GetChild(1).GetComponentInChildren<Image>();
            TextMeshProUGUI productQuantity = cartProduct.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>();

            // ��ǰ ������ ���� ǥ��
            productName.text = cartItem.product.name;
            productQuantity.text = $"x{cartItem.quantity}";
        }
    }


    public void OnBuyButtonClick()
    {
        int totalPrice = CalculateTotalPrice(); // ��ٱ����� �� ���� ���

        // �÷��̾��� ���� �� ���ݺ��� ���ų� ���� �� ���� ����
        if (playerMoney >= totalPrice)
        {
            playerMoney -= totalPrice; // �÷��̾� ������ �� ���� ����
            UpdatePlayerMoneyUI();     // UI ������Ʈ

            Debug.Log($"Items purchased for {totalPrice}. Remaining money: {playerMoney}");

            ClearCart(); // ��ٱ��� ����
        }
        else
        {
            Debug.Log("�������� �� ���� �����մϴ�.");
        }
    }

    public void ClearCart()
    {
        cartItems.Clear(); // ��ٱ��� ����
        GenerateCartProduct(); // UI ������Ʈ (��ٱ��ϸ� �ٽ� �׷��� ����� ���� ǥ��)
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

        // ��ٱ��Ͽ� ��� ��� ��ǰ�� �� ���� ���
        foreach (CartItem cartItem in cartItems)
        {
            totalPrice += cartItem.product.buyCost * cartItem.quantity; // ��ǰ ���� * ����
        }

        return totalPrice;
    }


}
