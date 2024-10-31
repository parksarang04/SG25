using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using static ShopManager;

public class ShopManager : MonoBehaviour
{
    public UIManager UIManager;

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
    public TextMeshProUGUI currentCartMoney;
    public TextMeshProUGUI currentCartCount;
    public ProductBoxGenerator ProductBoxGenerator;
    public ProductBox productBox;
    public GameObject productBoxObj;        //��ǰ ���� ������

    [Header("��ٱ��� �г�")]
    public int deliveryFee = 3000;
    public Button buyButton;    // ��ٱ��Ͽ��� '����'
    public List<ProductData> productDatas = new List<ProductData>();
    public GameObject CartProductContent;
    public GameObject CartProductPrefab;

    public TextMeshProUGUI totalProductPriceText;
    public TextMeshProUGUI totalPriceText;
    public TextMeshProUGUI deliveryFeeText;
    public TextMeshProUGUI remainingMoneyText;

    [Header("�÷��̾� �Ӵ�")]
    //public int playerMoney = 1000; // �ʱ� �÷��̾� �� 
    public TextMeshProUGUI PlayerMoneyText; // UI���� �÷��̾� ���� ǥ���ϴ� �ؽ�Ʈ
    public List<CartItem> cartItems = new List<CartItem>(); // ��ٱ��� �׸� ����Ʈ

    void Start()
    {
        UpdatePlayerMoneyUI();
        products = Resources.LoadAll<ProductData>("Products");  //���ҽ� ���Ͽ� �ִ� ProductDataŸ���� ��� products�迭�� �ִ´�.
        // OnCartPanelButtonClick();
        Generateproduct();
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

    // �÷��̾� �� UI ������Ʈ
    public void UpdatePlayerMoneyUI()
    {
        PlayerMoneyText.text = GameManager.Instance.playerMoney.ToString(); // �÷��̾��� ���� �ؽ�Ʈ�� ǥ��
        UIManager.moneyText.text = GameManager.Instance.playerMoney.ToString();
    }

    public void Generateproduct()
    {
        for (int i = 0; i < products.Length; i++)
        {
            GameObject productObj = Instantiate(productPrefab, productContent.transform);

            // Get references to the components
            TextMeshProUGUI productName = productObj.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
            Image image = productObj.transform.GetChild(2).GetComponentInChildren<Image>();
            TextMeshProUGUI price = productObj.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
            TMP_InputField count = productObj.transform.GetChild(4).GetComponentInChildren<TMP_InputField>();
            count.text = "1";

            Button plusBtn = productObj.transform.GetChild(5).GetComponentInChildren<Button>();
            Button minusBtn = productObj.transform.GetChild(6).GetComponentInChildren<Button>();
            Button CartBtn = productObj.transform.GetChild(7).GetComponentInChildren<Button>();
            int index = i;
            // Store a local copy of the count input field
            TMP_InputField localCount = count;
            CartBtn.onClick.AddListener(()=> CartBtnClick(count, products[index]));
            plusBtn.onClick.AddListener(() => CountUp(localCount));
            minusBtn.onClick.AddListener(() => CountDown(localCount));


            if (productObj != null && products[index] != null)
            {
                productName.text = products[index].name;               
                price.text = products[index].buyCost.ToString();
                image.sprite = products[index].image;
            }
        }


    }

    public void CartBtnClick(TMP_InputField count, ProductData product)
    {
        int productCount = int.Parse(count.text);

        if (productCount > 0)
        {
            // ��ٱ��Ͽ� �̹� ���� ��ǰ�� �ִ��� Ȯ��
            CartItem existingItem = cartItems.Find(item => item.product.ID == product.ID);

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
        UpdateCartTotal();
    }

    public void UpdateCartTotal()
    {
        int totalProductPrice = CalculateTotalPrice(); // ��ٱ��� �� ���� ���
        currentCartMoney.text = totalProductPrice.ToString(); // UI�� �Ѿ� ǥ��
        totalProductPriceText.text = totalProductPrice.ToString();

        int totalPrice = totalProductPrice + deliveryFee;
        totalPriceText.text = totalPrice.ToString();

        int remainingMoney = GameManager.Instance.playerMoney - totalPrice;
        remainingMoneyText.text = remainingMoney.ToString();

        int totalCount = cartItems.Count;
        currentCartCount.text = totalCount.ToString();
    }

    public void OnCartPanelButtonClick() //��ٱ��� ��ư�� Ŭ������ �� �Լ�
    {
        CartPanel.SetActive(true); //Activ�� true�� Ȱ��ȭ�� �ȴ�. false�� ��Ȱ��ȭ�� �ȴ�.
        GenerateCartProduct();
        deliveryFeeText.text = deliveryFee.ToString();
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
            TextMeshProUGUI productName = cartProduct.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
            //Image productImage = cartProduct.transform.GetChild(1).GetComponentInChildren<Image>();
            TextMeshProUGUI productQuantity = cartProduct.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
            Button plusButton = cartProduct.transform.GetChild(3).GetComponentInChildren<Button>(); //��ٱ��Ͽ��� '�����'
            Button minusButton = cartProduct.transform.GetChild(4).GetComponentInChildren<Button>(); //��ٱ��Ͽ��� '�����'
            TextMeshProUGUI productPrice = cartProduct.transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>();
            Button allRemoveButton = cartProduct.transform.GetChild(6).GetComponentInChildren<Button>();  //��ٱ��Ͽ��� '��� �����'
            

            minusButton.onClick.AddListener(() => CartMinus(cartItem));
            plusButton.onClick.AddListener(() => CartPlus(cartItem));
            allRemoveButton.onClick.AddListener(() => AllRemove(cartItem));

            // ��ǰ ������ ���� ǥ��
            productName.text = cartItem.product.name;
            productQuantity.text = $"x{cartItem.quantity}";
            productPrice.text = cartItem.product.buyCost.ToString();

            if (cartItem.quantity == 0 || cartItem == null)
            {
                Destroy(cartProduct.gameObject);
            } 
        }
    }


    public void OnBuyButtonClick()
    {
        int totalPrice = CalculateTotalPrice(); // ��ٱ����� �� ���� ���

        // �÷��̾��� ���� �� ���ݺ��� ���ų� ���� �� ���� ����
        if (GameManager.Instance.playerMoney >= (totalPrice + deliveryFee))
        {
            GameManager.Instance.playerMoney -= (totalPrice + deliveryFee); // �÷��̾� ������ �� ���� ����
            UpdatePlayerMoneyUI();     // UI ������Ʈ

            Debug.Log($"Items purchased for {totalPrice}. Remaining money: {GameManager.Instance.playerMoney}");

            foreach (CartItem cartItem in cartItems)
            {
                for (int i = 0; i < cartItem.quantity; i++)
                {
                    OnProductButtonClick(cartItem.product);
                }
            }

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

    public void CartMinus(CartItem cartItem)
    {
        cartItem.quantity--;

        if (cartItem.quantity <= 0)
        {
            cartItems.Remove(cartItem);
        }

        GenerateCartProduct();
        UpdateCartTotal();

        Debug.Log($"������ 1�� �����ؼ� {cartItem.quantity}�� ����~");
    }

    public void CartPlus(CartItem cartItem)
    {
        cartItem.quantity++;
        GenerateCartProduct();
        UpdateCartTotal();
    }

    public void AllRemove(CartItem cartItem)
    {
        cartItems.Remove(cartItem);

        GenerateCartProduct();

        Debug.Log("������ ���� ����");
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
    public void OnProductButtonClick(ProductData product)
    {
        var productInfo = new ProductBoxScriptObject();
        ProductBoxGenerator.GetOrder(productInfo, product);
        ProductBoxGenerator.GenerateProductBox(product);



        //GameObject BoxObj = Instantiate(productBoxObj, gameObject.transform); // ��ǰ �ڽ� �������� ����
        //ProductBox productBox = BoxObj.GetComponent<ProductBox>();
        //productBox.GenerationProduct(product); // �ڽ��� ��ǰ ���� ����
    }
}
