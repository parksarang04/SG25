using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestShop : MonoBehaviour
{
    public GameObject ProductListPanel;     //상품 목록 패널
    public GameObject ProductListContent;   //상품 목록 정렬창
    public ProductData[] products;          //상품 데이터
    public Button productButton;            //상품 버튼
    private ProductBox productBox;          //상품이 들어있는 상자
    public GameObject productBoxObj;        //상품 상자 프리팹

    private void Start()
    {
        products = Resources.LoadAll<ProductData>("");
        GenerateproductButton();
    }

    void GenerateproductButton()
    {
        for (int i = 0; i < products.Length; i++)
        {
            int index = i;
            Button productBtn = Instantiate(productButton, ProductListContent.transform);   //상품 버튼을 복제한다 어ㄷ
            TextMeshProUGUI productName = productBtn.GetComponentInChildren<TextMeshProUGUI>(); 
            SpriteRenderer productImage = productBtn.GetComponent<SpriteRenderer>();

            if (productBtn != null && products[index] != null)
            {
                productName.text = products[index].name;
                productImage.sprite = products[index].image;
                productBtn.onClick.AddListener(() => OnProductButtonClick(products[index]));
            }
        }
    }

    public void OnProductButtonClick(ProductData product)
    {
        GameObject BoxObj = Instantiate(productBoxObj);
        ProductBox productBox = BoxObj.GetComponent<ProductBox>();
        productBox.GenerationProduct(product);
    }
}
