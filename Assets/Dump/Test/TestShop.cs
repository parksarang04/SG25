using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestShop : MonoBehaviour
{
    public GameObject ProductListPanel;     //��ǰ ��� �г�
    public GameObject ProductListContent;   //��ǰ ��� ����â
    public ProductData[] products;          //��ǰ ������
    public Button productButton;            //��ǰ ��ư
    private ProductBox productBox;          //��ǰ�� ����ִ� ����
    public GameObject productBoxObj;        //��ǰ ���� ������

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
            Button productBtn = Instantiate(productButton, ProductListContent.transform);   //��ǰ ��ư�� �����Ѵ� �
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
